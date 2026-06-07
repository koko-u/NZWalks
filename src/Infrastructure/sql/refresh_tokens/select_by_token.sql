SELECT "id",
       "user_id",
       "token_hash",
       "expires_at"
FROM "refresh_tokens"
WHERE "token_hash" = @TokenHash
  AND "expires_at" IS NULL;