-- SEQUENCES
CREATE SEQUENCE FORUM_PARTITION_SEQ START WITH 1;
CREATE SEQUENCE USER_ROLE_SEQ START WITH 1;
CREATE SEQUENCE FORUM_USER_SEQ START WITH 1;
CREATE SEQUENCE FORUM_CATEGORY_SEQ START WITH 1;
CREATE SEQUENCE TOPIC_SEQ START WITH 1;
CREATE SEQUENCE FORUM_MESSAGE_SEQ START WITH 1;

-- TABLES
CREATE TABLE FORUM_PARTITION
(
    ID INT NOT NULL,
    NAME VARCHAR2(255) NOT NULL,
    CONSTRAINT PK_PARTITION PRIMARY KEY(ID)
);
CREATE TABLE USER_ROLE
(
    ID INT NOT NULL,
    NAME VARCHAR2(255) NOT NULL,
    CONSTRAINT PK_ROLE PRIMARY KEY(ID)
);
CREATE TABLE FORUM_USER
(
    ID INT NOT NULL,
    LOGIN VARCHAR2(255) NOT NULL,
    EMAIL VARCHAR2(255) NOT NULL,
    PASSWORD VARCHAR2(255) NOT NULL,
    REGISTRATION_DATE DATE NOT NULL,
    ROLE_ID INT,
    CONSTRAINT PK_USER PRIMARY KEY(ID),
    CONSTRAINT UNIQUE_EMAIL UNIQUE(EMAIL),
    CONSTRAINT UNIQUE_LOGIN UNIQUE(LOGIN),
    CONSTRAINT FK_USER_ROLE FOREIGN KEY (ROLE_ID)
    	REFERENCES USER_ROLE(ID)
);
CREATE TABLE FORUM_CATEGORY
(
    ID INT NOT NULL,
    PARTITION_ID INT NOT NULL,
    NAME VARCHAR2(255) NOT NULL,
    DESCRIPTION VARCHAR2(255),
    CONSTRAINT PK_CATEGORY PRIMARY KEY(ID),
    CONSTRAINT FK_CATEGORY_PARTITION FOREIGN KEY (PARTITION_ID)
    	REFERENCES FORUM_PARTITION(ID) ON DELETE CASCADE
);
CREATE TABLE TOPIC
(
    ID INT NOT NULL,
    NAME VARCHAR2(255) NOT NULL,
    CATEGORY_ID INT NOT NULL,
    USER_ID INT NOT NULL,
    PUBLISH_DATE DATE NOT NULL,
    IS_CLOSED NUMBER(1,0) NOT NULL,
    CONSTRAINT PK_TOPIC PRIMARY KEY(ID),
    CONSTRAINT FK_TOPIC_CATEGORY FOREIGN KEY(CATEGORY_ID)
    	REFERENCES FORUM_CATEGORY(ID),
    CONSTRAINT FK_TOPIC_USER FOREIGN KEY(USER_ID)
    	REFERENCES FORUM_USER(ID)
);
CREATE TABLE FORUM_MESSAGE
(
    ID INT NOT NULL,
    TEXT VARCHAR2(255) NOT NULL,
    TOPIC_ID INT NOT NULL,
    USER_ID INT NOT NULL,
    PUBLISH_DATE DATE NOT NULL,
    CONSTRAINT PK_FORUM_MESSAGE PRIMARY KEY(ID),
    CONSTRAINT FK_FORUM_MESSAGE_TOPIC FOREIGN KEY(TOPIC_ID)
    	REFERENCES TOPIC(ID),
    CONSTRAINT FK_FORUM_MESSAGE_USER FOREIGN KEY(USER_ID)
    	REFERENCES FORUM_USER(ID) 
);

-- TRIGGERS
CREATE OR REPLACE TRIGGER AUTOINCREMENT_FORUM_PARTITION 
BEFORE INSERT ON FORUM_PARTITION
FOR EACH ROW
BEGIN
  SELECT FORUM_PARTITION_SEQ.NEXTVAL
  INTO   :NEW.ID
  FROM   DUAL;
END;
/
CREATE OR REPLACE TRIGGER AUTOINCREMENT_USER_ROLE 
BEFORE INSERT ON USER_ROLE
FOR EACH ROW
BEGIN
  SELECT USER_ROLE_SEQ.NEXTVAL
  INTO   :NEW.ID
  FROM   DUAL;
END;
/
CREATE OR REPLACE TRIGGER AUTOINCREMENT_FORUM_USER 
BEFORE INSERT ON FORUM_USER
FOR EACH ROW
BEGIN
  SELECT FORUM_USER_SEQ.NEXTVAL
  INTO   :NEW.ID
  FROM   DUAL;
END;
/
CREATE OR REPLACE TRIGGER AUTOINCREMENT_FORUM_CATEGORY 
BEFORE INSERT ON FORUM_CATEGORY
FOR EACH ROW
BEGIN
  SELECT FORUM_CATEGORY_SEQ.NEXTVAL
  INTO   :NEW.ID
  FROM   DUAL;
END;
/
CREATE OR REPLACE TRIGGER AUTOINCREMENT_TOPIC 
BEFORE INSERT ON TOPIC
FOR EACH ROW
BEGIN
  SELECT TOPIC_SEQ.NEXTVAL
  INTO   :NEW.ID
  FROM   DUAL;
END;
/
CREATE OR REPLACE TRIGGER AUTOINCREMENT_FORUM_MESSAGE 
BEFORE INSERT ON FORUM_MESSAGE
FOR EACH ROW
BEGIN
  SELECT FORUM_MESSAGE_SEQ.NEXTVAL
  INTO   :NEW.ID
  FROM   DUAL;
END;