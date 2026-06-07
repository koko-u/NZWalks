INSERT INTO "users" ("email",
                     "display_name",
                     "password_hash")
VALUES (@Email,
        @DisplayName,
        @PasswordHash)
RETURNING "id",
    "email",
    "display_name";