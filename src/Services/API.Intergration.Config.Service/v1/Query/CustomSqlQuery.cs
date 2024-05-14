namespace API.Intergration.Config.Service.v1.Query;

public static class CustomSqlQuery
{
    public const string GetUserIntConfig = @"SELECT
                                                ui.user_id AS UserId,
                                                ui.partner_code AS PartnerCode,
                                                ui.partner_type AS PartnerType,
                                                jsonb_agg(jsonb_build_object('MethodKey', ui.method_key, 'MethodValue', ui.method_value)) AS MethodGroup
                                            FROM
                                                user_info.""user"".user_infomation_config ui
                                            WHERE
                                                ui.is_deleted = false
                                                AND ui.user_id = @userID
                                                AND ui.partner_code = @partnerCode
                                                AND ui.partner_type = @partnerType
                                            GROUP BY
                                                ui.user_id, ui.partner_code, ui.partner_type;
";
}