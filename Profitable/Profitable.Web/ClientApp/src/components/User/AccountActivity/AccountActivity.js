import { useEffect, useReducer, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, POSTS_LIST_POSTS_IN_PAGE_COUNT } from '../../../common/config';
import { loadPostsPageByUserId } from '../../../services/posts/postsService';
import { CommentsList } from '../../PostsAndComments/Comments/CommentsList/CommentsList';
import { PostsList } from '../../PostsAndComments/Posts/PostsList/PostsList';
import { getCommentsByUserId } from '../../../services/comments/commentsService';
import { AuthContext } from '../../../contexts/AuthContext';
import { MessageBoxContext } from '../../../contexts/MessageBoxContext';
import { getUserDataByJWT } from '../../../services/users/usersService';


import styles from './AccountActivity.module.css';

const reducer = (state, action) => {
    switch (action.type) {
        case 'changeActivityType':
            if(action.payload == 'posts') {
                return {
                    ...state,
                    activityType: action.payload.activityType,
                    activityList: action.payload.activityList
                };
            }
            else {
                return {
                    ...state,
                    activityType: action.payload.activityType,
                    activityList: action.payload.activityList
                };
            }
        case 'updateUserId':
            return {
                ...state,
                userId: action.payload.userId
            };
    }
}

export const AccountActivity = () => {

    const navigate = useNavigate();

    const [state, setState] = useReducer(reducer, {
        activityType: 'posts',
        activityList: [],
        page: 0,
        userId: '',
    });

    const { JWT, removeAuth } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    useEffect(() => {
        getUserDataByJWT(JWT)
            .then(result => {

                setState({
                    type: 'updateUserId',
                    payload: {
                        userId: result.guid,
                    }
                });
                
                loadPostsPageByUserId(JWT, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT)
                .then(result => {
                    setState({
                        type: 'changeActivityType',
                        payload: {
                            activityType: state.activityType,
                            activityList: result
                        }
                    });
                });
                
            })
            .catch(err => {
                if (JWT && err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    removeAuth();
                    navigate('/login');
                } else {
                    setMessageBoxSettings('You should login before creating a post!', false);
                    navigate('/login');
                }
            });
    }, []);

    const handleActivityTypeChange = (e) => {
        if(e.target.value === 'posts') {
            loadPostsPageByUserId(JWT, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT)
            .then(result => {
                console.log(result);
                setState({
                    type: 'changeActivityType',
                    payload: {
                        activityType: 'posts',
                        activityList: result
                    }
                });
            });
        } else if (e.target.value === 'comments') {
            getCommentsByUserId(JWT, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT)
                .then(result => {
                console.log(result);
                    setState({
                        type: 'changeActivityType',
                        payload: {
                            activityType: 'comments',
                            activityList: result
                        }
                    });
                });
        }
    }
    
    return (
        <div className={styles.ActivityContainer}>
            <select
                onChange={handleActivityTypeChange}
                value={state.activityType}
                className={styles.activityTypeSelectorType}>
                <option value="posts">Posts</option>
                <option value="comments">Comments</option>
            </select>

            <section className={styles.listContainer}>
               {
                    state.activityType == 'posts' ?
                        <PostsList posts={state.activityList} />
                    :
                        <CommentsList comments={state.activityList} />}
            </section>
        </div>
    );
}