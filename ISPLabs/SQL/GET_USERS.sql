CREATE OR REPLACE PROCEDURE GET_USERS(
        RESULT_USERS OUT NOCOPY SYS_REFCURSOR)
AS
BEGIN
    OPEN RESULT_USERS FOR
    SELECT FORUM_USER.id AS USER_ID,
    login AS USER_LOGIN,
    email AS USER_EMAIL,
    registration_date AS USER_REG_DATE,
    role_id AS USER_ROLE_ID
    from FORUM_USER
    INNER JOIN USER_ROLE ON FORUM_USER.ROLE_ID = USER_ROLE.ID;
END;