WITH "target_id" AS (SELECT "user_id"
                     FROM "refresh_tokens"
                     WHERE "token_hash" = @TokenHash
                       AND "expires_at" IS NULL)
UPDATE "refresh_tokens" AS "R"
SET "expires_at" = NOW() AT TIME ZONE 'UTC'
FROM "target_id" AS "T"
WHERE "R"."user_id" = "T"."user_id"
  AND "R"."expires_at" IS NULL;