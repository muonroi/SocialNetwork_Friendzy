# Sử dụng một hình ảnh Node.js cơ bản
FROM node:14

# Thiết lập thư mục làm việc trong container
WORKDIR /src

# Sao chép package.json và package-lock.json vào thư mục làm việc
COPY package*.json ./

# Sao chép toàn bộ mã nguồn vào thư mục làm việc
COPY . .

# Đặt thư mục làm việc cho dịch vụ
WORKDIR /src/Services/agora_nodejs/server

# Cài đặt các phụ thuộc với tùy chọn retry và timeout, sử dụng cache nếu có thể
RUN npm config set fetch-retries 5 \
    && npm config set fetch-retry-mintimeout 20000 \
    && npm config set fetch-retry-maxtimeout 120000 \
    && npm install --prefer-offline

# Expose cổng mà ứng dụng của bạn sẽ chạy
EXPOSE 80

# Lệnh khởi động ứng dụng
CMD ["node", "agora_server.js"]