CREATE OR REPLACE FUNCTION REGISTRATION(PASS_LOGIN VARCHAR2,PASS_EMAIL VARCHAR2, PASS_PASSWORD VARCHAR2,PASS_ROLE_ID NUMBER, ERR OUT VARCHAR2)
RETURN NUMBER
IS
COUNT_USERS_LOGIN NUMBER;
COUNT_USERS_EMAIL NUMBER;
BEGIN
    SELECT COUNT(*) INTO COUNT_USERS_LOGIN FROM FORUM_USER 
    WHERE LOGIN = PASS_LOGIN;
    SELECT COUNT(*) INTO COUNT_USERS_EMAIL FROM FORUM_USER 
    WHERE EMAIL = PASS_EMAIL;
    IF COUNT_USERS_LOGIN != 0 THEN
        ERR:='SUCH LOGIN ALREADY EXIST';
        RETURN 0;
    ELSIF COUNT_USERS_EMAIL != 0 THEN
        ERR:='SUCH EMAIL ALREADY EXIST';
        RETURN 0;
    ELSE
        INSERT INTO FORUM_USER (LOGIN,EMAIL,PASSWORD,REGISTRATION_DATE,ROLE_ID)
        VALUES(PASS_LOGIN,PASS_EMAIL,PASS_PASSWORD,SYSDATE,PASS_ROLE_ID);
        RETURN 1;
    END IF;
END;
