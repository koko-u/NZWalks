-- migrate:up
CREATE TABLE IF NOT EXISTS "difficulties" (
    "id" UUID NOT NULL DEFAULT uuidv7(),
    "name" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMPTZ NOT NULL DEFAULT now(),
    "updated_at" TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT "difficulty_pkey" PRIMARY KEY ("id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "uq_difficulties_name" ON "difficulties"("name");

CREATE OR REPLACE TRIGGER "tgr_difficulties_updated_at"
    BEFORE UPDATE ON "difficulties"
    FOR EACH ROW
    EXECUTE PROCEDURE moddatetime("updated_at");


-- migrate:down
DROP TABLE IF EXISTS "difficulties";
