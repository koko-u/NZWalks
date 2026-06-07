-- migrate:up
CREATE TABLE IF NOT EXISTS "users" (
    "id" UUID NOT NULL DEFAULT uuidv7(),
    "email" VARCHAR(255) NOT NULL,
    "display_name" VARCHAR(255) NULL DEFAULT NULL,
    "password_hash" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMPTZ NOT NULL DEFAULT now(),
    "updated_at" TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT "users_pkey" PRIMARY KEY ("id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "uq_users_email" ON "users" ("email");

CREATE OR REPLACE TRIGGER "tgr_users_updated_at"
    BEFORE UPDATE ON "users"
    FOR EACH ROW
EXECUTE PROCEDURE moddatetime("updated_at");

-- migrate:down
DROP TABLE IF EXISTS "users";

