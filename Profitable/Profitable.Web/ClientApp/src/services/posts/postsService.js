import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';


export const loadParticularPost = (postId) => {
    return request.get(`${WEB_API_BASE_URL}/posts/${postId}`)
        .then(response => response.json());
}

export const createPost = async (jwt, post) => {
    let result = await request.post(`${WEB_API_BASE_URL}/posts/create`, post, {
        'Authorization': 'Bearer ' + jwt
    });

    if(result.status === 400) {
        throw new Error(await result.text());
    } else if(result.status === 401) {
        throw new Error('Should auth first');
    }

    return await result.json();
}

export const editPost = async (jwt, postGuid, post) => {
    let result = await request.patch(`${WEB_API_BASE_URL}/posts/${postGuid}/edit`, post, {
        'Authorization': 'Bearer ' + jwt
    });

    if(result.status === 400) {
        throw new Error(await result.text());
    } else if(result.status === 401) {
        throw new Error('Should auth first');
    }

    return await result.json();
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