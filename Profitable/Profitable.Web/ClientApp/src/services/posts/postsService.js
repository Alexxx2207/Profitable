import { WEB_API_BASE_URL } from '../../common/config';

export const createImgURL = (post) => {
    const imageUrl = `data:image/${post.imageType};base64,${post.image}`;
    return imageUrl;
}

export const loadParticularPost = (postId) => {
    return fetch(`${WEB_API_BASE_URL}/posts/${postId}`)
        .then(response => response.json());
}

export const loadPostsPage = (page) => {
    return fetch(`${WEB_API_BASE_URL}/posts/pages/${page}`)
        .then(response => response.json());
}