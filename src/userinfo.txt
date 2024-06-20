CREATE TABLE user_infomation_config (
    id uuid DEFAULT uuid_generate_v4() NOT NULL,
    user_id int,
    partner_code character varying(100),
    partner_type character varying(100),
    method_key character varying(100),
    method_value character varying(500),
    is_deleted boolean DEFAULT false NOT NULL,
    created_by character varying(64),
    created_date timestamp without time zone DEFAULT (now() AT TIME ZONE 'utc'::text) NOT NULL,
    updated_by character varying(64),
    updated_date timestamp without time zone,
    type character varying(50),
    key character varying(100),
    value character varying(250)
);

INSERT INTO user_infomation_config (id, user_id, partner_code, partner_type, method_key, method_value, is_deleted, created_by, created_date, updated_by, updated_date, type, key, value) VALUES
    ('3116cb66-3736-4521-a3ca-c41a2b2e2fd6', 1, 'Intergration', 'API', 'get-user', 'http://User.Service/api/v1/User', false, NULL, '2023-12-25 13:08:35.789853', NULL, NULL, NULL, NULL, NULL),
    ('3d6eadc4-8ba3-44da-aa1a-074e1bdeb3a9', 1, 'Intergration', 'API', 'get-users', 'http://User.Service/api/v1/User/list', false, NULL, '2023-12-25 13:08:35.789853', NULL, NULL, NULL, NULL, NULL),
    ('47028c6a-bf96-4071-8ba4-ff498158b823', 1, 'Intergration', 'API', 'get-category-setting', 'http://Setting.Service/api/v1/Setting/categories', false, NULL, '2023-12-25 13:08:35.789853', NULL, NULL, NULL, NULL, NULL),
    ('f9200919-fd24-4bda-bbab-6a4eacbdc30e', 1, 'Intergration', 'API', 'verify-account', 'http://Account.Service/api/v1/Account/verify', false, NULL, '2023-12-25 13:08:35.789853', NULL, NULL, NULL, NULL, NULL),
    ('ae62bc04-67f8-4ef1-be59-785e782bcf5d', 1, 'Intergration', 'API', 'create-account', 'http://Account.Service/api/v1/Account/create', false, NULL, '2023-12-25 13:08:35.789853', NULL, NULL, NULL, NULL, NULL),
    ('079c1671-3a4c-4fbe-a11a-c26dcdac676d', 1, 'Intergration', 'API', 'get-friends', 'http://Management.Friends.Action.Service/api/v1/ManagementFriendsAction/friends', false, NULL, '2023-12-25 13:08:35.789853', NULL, NULL, NULL, NULL, NULL),
    ('2fade1da-53df-4299-a809-72b7b7ec1373', 1, 'Intergration', 'API', 'get-friends-user', 'http://Management.Friends.Action.Service/api/v1/ManagementFriendsAction/friends-user', false, NULL, '2023-12-25 13:08:35.789853', NULL, NULL, NULL, NULL, NULL);
