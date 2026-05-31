-- migrate:up
CREATE TABLE IF NOT EXISTS "walks" (
    "id" UUID NOT NULL DEFAULT uuidv7(),
    "name" VARCHAR(255) NOT NULL,
    "description" TEXT NULL,
    "length_km" DOUBLE PRECISION NOT NULL,
    "image_url" VARCHAR(2048) NULL,
    "region_id" UUID NOT NULL,
    "difficulty_id" UUID NOT NULL,
    "created_at" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_at" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT "walks_pkey" PRIMARY KEY ("id"),
    CONSTRAINT "walks_region_id_fkey" FOREIGN KEY ("region_id") REFERENCES "regions" ("id") ON DELETE CASCADE,
    CONSTRAINT "walks_difficulty_id_fkey" FOREIGN KEY ("difficulty_id") REFERENCES "difficulties" ("id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS "uq_walks_name" ON "walks" ("name");
CREATE INDEX IF NOT EXISTS "idx_walks_region" ON "walks" ("region_id");
CREATE INDEX IF NOT EXISTS "idx_walks_difficulty" ON "walks" ("difficulty_id");

CREATE OR REPLACE TRIGGER "tgr_walks_updated_at"
    BEFORE UPDATE ON "walks"
    FOR EACH ROW
EXECUTE PROCEDURE moddatetime("updated_at");

-- migrate:down
DROP TABLE IF EXISTS "walks";
