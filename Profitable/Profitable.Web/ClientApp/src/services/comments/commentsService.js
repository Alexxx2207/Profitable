import { WEB_API_BASE_URL } from "../../common/config";
import { request } from "../../utils/fetch/request";

export const getCommentsByPostId = (postId, page, commentsCount) => {
    return request
        .get(`${WEB_API_BASE_URL}/comments/bypost/${postId}/all/${page}/${commentsCount}`)
        .then((response) => response.json());
};

export const getCommentsByUserId = (jwt, page, commentsCount) => {
    return request
        .get(`${WEB_API_BASE_URL}/comments/byuser/all/${page}/${commentsCount}`,
        null,
        {
            'Authorization': 'Bearer ' + jwt
        })
        .then((response) => response.json());
};

export const postComment = async (jwt, postId, comment) => {
    
    let result = await request
        .post(`${WEB_API_BASE_URL}/comments/${postId}/add`,
        {
            content: comment
        },
        {
            'Authorization': 'Bearer ' + jwt
        });

        console.log(comment);

    if([400, 401].includes(result.status)) {
        throw new Error(result.status);
    } else {
        return await result.json();
    }
};