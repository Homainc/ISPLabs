CREATE OR REPLACE FUNCTION INSERT_MESSAGE(PASS_TEXT VARCHAR2,PASS_TOPIC_ID NUMBER, PASS_USER_ID NUMBER, ERR OUT VARCHAR2)
RETURN NUMBER
IS
COUNT_TOPICS NUMBER;
COUNT_USERS NUMBER;
BEGIN
    SELECT COUNT(*) INTO COUNT_USERS FROM FORUM_USER 
    WHERE ID = PASS_USER_ID;
    SELECT COUNT(*) INTO COUNT_TOPICS FROM TOPIC 
    WHERE ID = PASS_TOPIC_ID;
    IF(COUNT_USERS = 0) THEN
        ERR:='SUCH USER NOT EXIST';
        RETURN 0;
    ELSIF(COUNT_TOPICS = 0) THEN
        ERR:='SUCH TOPIC NOT EXIST';
        RETURN 0;
    ELSE
        INSERT INTO FORUM_MESSAGE (TEXT,TOPIC_ID,USER_ID,PUBLISH_DATE)
        VALUES(PASS_TEXT,PASS_TOPIC_ID,PASS_USER_ID,SYSDATE);
        RETURN 1;
    END IF;
END;