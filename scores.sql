﻿CREATE TABLE scores(
    id INT PRIMARY KEY AUTO_INCREMENT,
    score INT NOT NULL,
    username VARCHAR(24) NOT NULL,
    game LONGBLOB NOT NULL,
    saved_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);