<?php
// Check if the Imagick extension is loaded
if (extension_loaded('imagick')) {
    echo 'Imagick is installed and enabled.';
} else {
    echo 'Imagick is NOT installed or enabled.';
}
