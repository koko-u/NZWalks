INSERT INTO "refresh_tokens" ("user_id",
                              "token_hash",
                              "expires_at")
VALUES (@UserId,
        @TokenHash,
        NULL);
