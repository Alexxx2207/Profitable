import { allowedImageSize, allowedImageTypes } from "../../common/validationConstants";
export const createImgURL = (image) => {
    const imageUrl = `data:image;base64,${image}`;
    return imageUrl;
};

export const createAuthorImgURL = (image) => {
    if (image) {
        return createImgURL(image);
    } else {
        return `${process.env.PUBLIC_URL}images/defaultProfilePicture.png`;
    }
};

export const validateImage = (image, setMessageBoxSettings) => {
    if (!allowedImageTypes.includes(image.type)) {
        setMessageBoxSettings("Image type is not valid", false);
        return false;
    } else if (image.size > allowedImageSize) {
        setMessageBoxSettings(
            `Image size is too big. It should be less than ${allowedImageSize / 1024 / 1024}MB`,
            false
        );
        return false;
    }

    return true;
};
