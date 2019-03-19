CREATE OR REPLACE FUNCTION encrypt_text(secure_text IN VARCHAR2, key_text IN VARCHAR2)
RETURN VARCHAR2
IS
    enc_val RAW(4000);
BEGIN
    enc_val :=
      dbms_crypto.ENCRYPT (
        src => utl_i18n.string_to_raw (secure_text, 'AL32UTF8'),
        KEY => utl_i18n.string_to_raw (key_text, 'AL32UTF8'),
        typ =>   dbms_crypto.encrypt_aes128
                    + dbms_crypto.chain_cbc
                    + dbms_crypto.pad_pkcs5
      );
   RETURN enc_val;
END;