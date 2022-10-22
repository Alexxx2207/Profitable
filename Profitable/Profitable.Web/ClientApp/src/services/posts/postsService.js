import { WEB_API_BASE_URL } from '../../common/config';
import { request } from '../../utils/fetch/request';


export const loadParticularPost = async (postId, userEmail) => {
    return await request.get(`${WEB_API_BASE_URL}/posts/${postId}?loggedInUserEmail=${userEmail}`)
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

export const deletePost = async (jwt, postGuid) => {
    let result = await request.delete(`${WEB_API_BASE_URL}/posts/${postGuid}/delete`, null, {
        'Authorization': 'Bearer ' + jwt
    });

    if(result.status === 400) {
        throw new Error(await result.text());
    } else if(result.status === 401) {
        throw new Error('Should auth first');
    }

    return await result.json();
}

export const loadPostsPage = (page, pageCount, userEmail) => {
    return request.get(
        `${WEB_API_BASE_URL}/posts/feed/${page}/${pageCount}?loggedInUserEmail=${userEmail}`)
        .then(response => response.json());
};

export const loadPostsPageByUserId = (jwt, page, pageCount) => {
    return request.get(
        `${WEB_API_BASE_URL}/posts/byuser/all/${page}/${pageCount}`,
        null,
        {
            'Authorization': 'Bearer ' + jwt
        }
    )
        .then(response => response.json());
};

export const manageLikePost = async (jwt, postId) => {
    let result = await request.post(`${WEB_API_BASE_URL}/posts/${postId}/likes/manage`, null, {
        'Authorization': 'Bearer ' + jwt
    });

    if(result.status === 400) {
        throw new Error(await result.text());
    } else if(result.status === 401) {
        throw new Error('Should auth first');
    }

    return await result.text();
}