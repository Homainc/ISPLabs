CREATE TABLE FORUM_MESSAGE_JOURNAL
( 
    OP_DATE DATE,
    OP_TYPE VARCHAR2(20),
    OLD_ID NUMBER,
    OLD_TEXT VARCHAR2(255),
    OLD_TOPIC_ID NUMBER,
    OLD_USER_ID NUMBER,
    OLD_PUBLISH_DATE DATE,
    NEW_ID NUMBER,
    NEW_TEXT VARCHAR2(255),
    NEW_TOPIC_ID NUMBER,
    NEW_USER_ID NUMBER,
    NEW_PUBLISH_DATE DATE
)
/
CREATE TABLE FORUM_TOPIC_JOURNAL
(
    OP_DATE DATE,
    OP_TYPE VARCHAR2(20),
    OLD_ID NUMBER,
    OLD_NAME VARCHAR2(255),
    OLD_CATEGORY_ID NUMBER,
    OLD_USER_ID NUMBER,
    OLD_PUBLISH_DATE DATE,
    OLD_IS_CLOSED NUMBER(1, 0),
    NEW_ID NUMBER,
    NEW_NAME VARCHAR2(255),
    NEW_CATEGORY_ID NUMBER,
    NEW_USER_ID NUMBER,
    NEW_PUBLISH_DATE DATE,
    NEW_IS_CLOSED NUMBER
)
/
CREATE TABLE FORUM_USER_JOURNAL
(
    OP_DATE DATE,
    OP_TYPE VARCHAR2(20),
    OLD_ID NUMBER,
    OLD_LOGIN VARCHAR2(255),
    OLD_EMAIL VARCHAR2(255),
    OLD_PASSWORD VARCHAR2(255),
    OLD_REGISTRATION_DATE DATE,
    OLD_ROLE_ID NUMBER,
    NEW_ID NUMBER,
    NEW_LOGIN VARCHAR2(255),
    NEW_EMAIL VARCHAR2(255),
    NEW_PASSWORD VARCHAR2(255),
    NEW_REGISTRATION_DATE DATE,
    NEW_ROLE_ID NUMBER
)
/
CREATE OR REPLACE TRIGGER FORUM_MESSAGE_JOURNALING
    AFTER INSERT OR UPDATE OR DELETE ON FORUM_MESSAGE
    FOR EACH ROW
DECLARE
    REC_TABLE FORUM_MESSAGE_JOURNAL%ROWTYPE;
    operation_type varchar2(20);
BEGIN
    CASE
        WHEN INSERTING THEN
            REC_TABLE.OP_TYPE :='insert';
        WHEN UPDATING THEN
            REC_TABLE.OP_TYPE :='update';
        WHEN DELETING THEN
            REC_TABLE.OP_TYPE :='delete';
    END CASE;
    REC_TABLE.OP_DATE := SYSDATE;
    REC_TABLE.OLD_ID := :OLD.ID;
    REC_TABLE.OLD_TEXT := :OLD.TEXT;
    REC_TABLE.OLD_TOPIC_ID := :OLD.TOPIC_ID;
    REC_TABLE.OLD_USER_ID := :OLD.USER_ID;
    REC_TABLE.OLD_PUBLISH_DATE := :OLD.PUBLISH_DATE;
    REC_TABLE.NEW_ID := :NEW.ID;
    REC_TABLE.NEW_TEXT := :NEW.TEXT;
    REC_TABLE.NEW_TOPIC_ID := :NEW.TOPIC_ID;
    REC_TABLE.NEW_USER_ID := :NEW.USER_ID;
    REC_TABLE.NEW_PUBLISH_DATE := :NEW.PUBLISH_DATE;
    INSERT INTO FORUM_MESSAGE_JOURNAL VALUES REC_TABLE;
END;
/
CREATE OR REPLACE TRIGGER FORUM_TOPIC_JOURNALING
    AFTER INSERT OR UPDATE OR DELETE ON TOPIC
    FOR EACH ROW
DECLARE
    REC_TABLE FORUM_TOPIC_JOURNAL%ROWTYPE;
    operation_type varchar2(20);
BEGIN
    CASE
        WHEN INSERTING THEN
            REC_TABLE.OP_TYPE :='insert';
        WHEN UPDATING THEN
            REC_TABLE.OP_TYPE :='update';
        WHEN DELETING THEN
            REC_TABLE.OP_TYPE :='delete';
    END CASE;
    REC_TABLE.OP_DATE := SYSDATE;
    REC_TABLE.OLD_ID := :OLD.ID;
    REC_TABLE.OLD_NAME := :OLD.NAME;
    REC_TABLE.OLD_CATEGORY_ID := :OLD.CATEGORY_ID;
    REC_TABLE.OLD_USER_ID := :OLD.USER_ID;
    REC_TABLE.OLD_PUBLISH_DATE := :OLD.PUBLISH_DATE;
    REC_TABLE.OLD_IS_CLOSED := :OLD.IS_CLOSED;
    REC_TABLE.NEW_ID := :NEW.ID;
    REC_TABLE.NEW_NAME := :NEW.NAME;
    REC_TABLE.NEW_CATEGORY_ID := :NEW.CATEGORY_ID;
    REC_TABLE.NEW_USER_ID := :NEW.USER_ID;
    REC_TABLE.NEW_PUBLISH_DATE := :NEW.PUBLISH_DATE;
    REC_TABLE.NEW_IS_CLOSED := :NEW.IS_CLOSED;
    INSERT INTO FORUM_TOPIC_JOURNAL VALUES REC_TABLE;
END;
/
CREATE OR REPLACE TRIGGER FORUM_USER_JOURNALING
    AFTER INSERT OR UPDATE OR DELETE ON FORUM_USER
    FOR EACH ROW
DECLARE
    REC_TABLE FORUM_USER_JOURNAL%ROWTYPE;
    operation_type varchar2(20);
BEGIN
    CASE
        WHEN INSERTING THEN
            REC_TABLE.OP_TYPE :='insert';
        WHEN UPDATING THEN
            REC_TABLE.OP_TYPE :='update';
        WHEN DELETING THEN
            REC_TABLE.OP_TYPE :='delete';
    END CASE;
    REC_TABLE.OP_DATE := SYSDATE;
    REC_TABLE.OLD_ID := :OLD.ID;
    REC_TABLE.OLD_LOGIN := :OLD.LOGIN;
    REC_TABLE.OLD_EMAIL := :OLD.EMAIL;
    REC_TABLE.OLD_PASSWORD := :OLD.PASSWORD;
    REC_TABLE.OLD_REGISTRATION_DATE := :OLD.REGISTRATION_DATE;
    REC_TABLE.OLD_ROLE_ID := :OLD.ROLE_ID;
    REC_TABLE.NEW_ID := :NEW.ID;
    REC_TABLE.NEW_LOGIN := :NEW.LOGIN;
    REC_TABLE.NEW_EMAIL := :NEW.EMAIL;
    REC_TABLE.NEW_PASSWORD := :NEW.PASSWORD;
    REC_TABLE.NEW_REGISTRATION_DATE := :NEW.REGISTRATION_DATE;
    REC_TABLE.NEW_ROLE_ID := :NEW.ROLE_ID;
    INSERT INTO FORUM_USER_JOURNAL VALUES REC_TABLE;
END;
/
CREATE TABLE FORUM_CATEGORY_JOURNAL
(
    OP_TYPE VARCHAR(20),
    OP_DATE DATE,
    OLD_ID NUMBER,
    OLD_PARTITION_ID NUMBER,
    OLD_NAME VARCHAR(255),
    OLD_DESCRIPTION VARCHAR(255),
    NEW_ID NUMBER,
    NEW_PARTITION_ID NUMBER,
    NEW_NAME VARCHAR(255),
    NEW_DESCRIPTION VARCHAR(255)
)
/
CREATE OR REPLACE TRIGGER FORUM_CATEGORY_JOURNALING
    AFTER INSERT OR UPDATE OR DELETE ON FORUM_CATEGORY
    FOR EACH ROW
DECLARE
    REC_TABLE FORUM_CATEGORY_JOURNAL%ROWTYPE;
    operation_type varchar2(20);
BEGIN
    CASE
        WHEN INSERTING THEN
            REC_TABLE.OP_TYPE :='insert';
        WHEN UPDATING THEN
            REC_TABLE.OP_TYPE :='update';
        WHEN DELETING THEN
            REC_TABLE.OP_TYPE :='delete';
    END CASE;
    REC_TABLE.OP_DATE := SYSDATE;
    REC_TABLE.OLD_ID := :OLD.ID;
    REC_TABLE.OLD_PARTITION_ID := :OLD.PARTITION_ID;
    REC_TABLE.OLD_NAME := :OLD.NAME;
    REC_TABLE.OLD_DESCRIPTION := :OLD.DESCRIPTION;
    REC_TABLE.NEW_ID := :NEW.ID;
    REC_TABLE.NEW_PARTITION_ID := :NEW.PARTITION_ID;
    REC_TABLE.NEW_NAME := :NEW.NAME;
    REC_TABLE.NEW_DESCRIPTION := :NEW.DESCRIPTION;
    INSERT INTO FORUM_CATEGORY_JOURNAL VALUES REC_TABLE;
END;
/
CREATE TABLE FORUM_PARTITION_JOURNAL
(
    OP_TYPE VARCHAR(20),
    OP_DATE DATE,
    OLD_ID NUMBER,
    OLD_NAME VARCHAR(255),
    NEW_ID NUMBER,
    NEW_NAME VARCHAR(255)
)
/
CREATE OR REPLACE TRIGGER FORUM_PARTITION_JOURNALING
    AFTER INSERT OR UPDATE OR DELETE ON FORUM_PARTITION
    FOR EACH ROW
DECLARE
    REC_TABLE FORUM_PARTITION_JOURNAL%ROWTYPE;
    operation_type varchar2(20);
BEGIN
    CASE
        WHEN INSERTING THEN
            REC_TABLE.OP_TYPE :='insert';
        WHEN UPDATING THEN
            REC_TABLE.OP_TYPE :='update';
        WHEN DELETING THEN
            REC_TABLE.OP_TYPE :='delete';
    END CASE;
    REC_TABLE.OP_DATE := SYSDATE;
    REC_TABLE.OLD_ID := :OLD.ID;
    REC_TABLE.OLD_NAME := :OLD.NAME;
    REC_TABLE.NEW_ID := :NEW.ID;
    REC_TABLE.NEW_NAME := :NEW.NAME;
    INSERT INTO FORUM_PARTITION_JOURNAL VALUES REC_TABLE;
END;
/