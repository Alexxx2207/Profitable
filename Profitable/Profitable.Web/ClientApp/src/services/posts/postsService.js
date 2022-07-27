import { WEB_API_BASE_URL } from '../../common/config';

export const createImgURL = (imageType, image) => {
    const imageUrl = `data:image/${imageType};base64,${image}`;
    return imageUrl;
}

export const createAuthorImgURL = (imageType, image) => {
    if(image != '') {
        return createImgURL(imageType, image);
    } else {
        return `${process.env.PUBLIC_URL}images/defaultProfilePicture.png`;
    }
}

export const loadParticularPost = (postId) => {
    return fetch(`${WEB_API_BASE_URL}/posts/${postId}`)
        .then(response => response.json());
}

export const loadPostsPage = (page) => {
    return fetch(`${WEB_API_BASE_URL}/posts/pages/${page}`)
        .then(response => response.json());
}