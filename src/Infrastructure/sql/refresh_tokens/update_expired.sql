UPDATE "refresh_tokens"
SET "expires_at" = NOW() AT TIME ZONE 'UTC'
WHERE "user_id" = @UserId
  AND "expires_at" IS NULL;