var http = require('http');
var express = require('express');
var { RtcTokenBuilder, RtmTokenBuilder, RtcRole, RtmRole } = require('agora-access-token');

const crypto = require('crypto');

console.log(`Server will start on port: ${process.env.Agora_AppId}`); 


const port = parseInt(process.env.Agora_Port);
const appID = getEx(process.env.Agora_AppId);
const appCertificate = getEx(process.env.Agora_AppCertificate);
const expirationTimeInSeconds = parseInt(process.env.Agora_EXPIRATION_TIME_IN_SECONDS);
const role = RtcRole.PUBLISHER;

var app = express();
app.disable('x-powered-by');
app.set('port', port);
app.use(express.json());

/**
 * Hàm tạo token RTC
 * @param {object} req - Yêu cầu từ client
 * @param {object} resp - Đáp ứng trả về client
 */
var generateRtcToken = function(req, resp) {
    var currentTimestamp = Math.floor(Date.now() / 1000);
    var privilegeExpiredTs = currentTimestamp + expirationTimeInSeconds;
    var channelName = req.query.channelName;
    var uid = req.query.uid || 0;

    if (!channelName) {
        return resp.status(400).json({ 'error': 'channel name is required' }).send();
    }

    var key = RtcTokenBuilder.buildTokenWithUid(appID, appCertificate, channelName, uid, role, privilegeExpiredTs);

    resp.header("Access-Control-Allow-Origin", "*");
    return resp.json({ 'key': key }).send();
};

/**
 * Hàm tạo token RTM
 * @param {object} req - Yêu cầu từ client
 * @param {object} resp - Đáp ứng trả về client
 */
var generateRtmToken = function(req, resp) {
    var currentTimestamp = Math.floor(Date.now() / 1000);
    var privilegeExpiredTs = currentTimestamp + expirationTimeInSeconds;
    var account = req.query.account;

    if (!account) {
        return resp.status(400).json({ 'error': 'account is required' }).send();
    }

    var key = RtmTokenBuilder.buildToken(appID, appCertificate, account, RtmRole.Rtm_User, privilegeExpiredTs);

    resp.header("Access-Control-Allow-Origin", "*");
    return resp.json({ 'key': key }).send();
};

/**
 * Hàm để lấy key hợp lệ
 * @param {string} key - Khóa ban đầu.
 * @param {number} keySize - Kích thước khóa cần thiết (256-bit cho AES-256).
 * @returns {Buffer} - Khóa hợp lệ có kích thước 256-bit.
 */
function getValidKey(key, keySize) {
    const keyBytes = Buffer.from(key, 'utf8');
    const hashBytes = crypto.createHash('sha256').update(keyBytes).digest();
    const validKey = hashBytes.slice(0, keySize / 8);
    return validKey;
}
/**
 * Hàm mã hóa văn bản bằng AES-256-CBC với khóa bí mật cố định và IV cố định.
 * @param {string} key - Khóa bí mật.
 * @param {string} plainText - Văn bản cần mã hóa.
 * @returns {string} - Văn bản đã mã hóa dưới dạng Base64.
 */

function encrypt(key, plainText) {
    if (!plainText || !key) {
        throw new Error('Key and plainText must not be null or empty.');
    }

    const keyBytes = getValidKey(key, 256);
    const iv = Buffer.alloc(16, 0); // Sử dụng IV cố định

    const cipher = crypto.createCipheriv('aes-256-cbc', keyBytes, iv);
    let encrypted = cipher.update(plainText, 'utf8', 'base64');
    encrypted += cipher.final('base64');

    return encrypted;
}

/**
 * Hàm giải mã văn bản đã mã hóa bằng AES-256-CBC với khóa bí mật cố định và IV cố định.
 * @param {string} key - Khóa bí mật.
 * @param {string} cipherText - Văn bản đã mã hóa.
 * @returns {string} - Văn bản gốc đã giải mã.
 */
function decrypt(key, cipherText) {
    if (!cipherText || !key) {
        throw new Error('Key and cipherText must not be null or empty.');
    }

    const keyBytes = getValidKey(key, 256);
    const iv = Buffer.alloc(16, 0); // Sử dụng IV cố định

    const decipher = crypto.createDecipheriv('aes-256-cbc', keyBytes, iv);
    let decrypted = decipher.update(cipherText, 'base64', 'utf8');
    decrypted += decipher.final('utf8');

    return decrypted;
}

/**
 * Hàm để lấy và giải mã văn bản từ cấu hình.
 * @param {string} secretKey - Khóa bí mật để giải mã văn bản.
 * @returns {string|null} - Văn bản gốc đã giải mã hoặc null nếu không thành công.
 */
function getEx(text) {
    var secretKey = process.env.SECRET_KEY;
    if (!text || !secretKey) {
        throw new Error('text and secretKey must not be null or empty.');
    }

    const cipherText = text;
    if (!cipherText) {
        throw new Error('cipherText must not be null or empty.');
    }

    const plainText = decrypt(secretKey, cipherText);
    if (!plainText) {
        throw new Error('plainText must not be null or empty.');
    }

    return plainText;
}

app.get('/rtcToken', generateRtcToken);
app.get('/rtmToken', generateRtmToken);

http.createServer(app).listen(app.get('port'), function() {
    console.log('AgoraSignServer starts at ' + app.get('port'));
});
