import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';
export const createImgURL = (imageType, image) => {
    const imageUrl = `data:image/${imageType};base64,${image}`;
    return imageUrl;
}

export const createAuthorImgURL = (imageType, image) => {
    if (image !== '') {
        return createImgURL(imageType, image);
    } else {
        return `${process.env.PUBLIC_URL}images/defaultProfilePicture.png`;
    }
}

export const loadParticularPost = (postId) => {
    return request.get(`${WEB_API_BASE_URL}/posts/${postId}`)
        .then(response => response.json());
}

export const loadPostsPage = (page, postsCount) => {

    const postBody = {
        'PostsCount': postsCount,
        'Page': page
    };

    return request.post(
        `${WEB_API_BASE_URL}/posts/pages`,
        postBody
    )
        .then(response => response.json());
}

export const likePost = (postId) => {
    // AJAX for Like (USER SHOULD BE AUTHENTICATED)
    console.log('works');
}