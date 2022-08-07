
export const createImgURL = (image) => {
    const imageUrl = `data:image;base64,${image}`;
    return imageUrl;
}

export const createAuthorImgURL = (image) => {
    if (image !== '') {
        return createImgURL(image);
    } else {
        return `${process.env.PUBLIC_URL}images/defaultProfilePicture.png`;
    }
}