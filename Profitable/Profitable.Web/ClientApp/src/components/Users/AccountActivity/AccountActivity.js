import { useEffect, useReducer, useContext, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import {
    ACTIVITY_TYPE_COMMENTS,
    ACTIVITY_TYPE_POSTS,
    COMMENTS_LIST_IN_POST_PAGE_COUNT,
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    POSTS_LIST_POSTS_IN_PAGE_COUNT,
} from "../../../common/config";
import { loadPostsPageByUserId } from "../../../services/posts/postsService";
import { CommentsList } from "../../PostsAndComments/Comments/CommentsList/CommentsList";
import { PostsList } from "../../PostsAndComments/Posts/PostsList/PostsList";
import { getCommentsByUserId } from "../../../services/comments/commentsService";
import { AuthContext } from "../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";
import { getUserDataByJWT } from "../../../services/users/usersService";

import styles from "./AccountActivity.module.css";

const reducer = (state, action) => {
    switch (action.type) {
        case "changeActivityType":
            if (action.payload.activityType === ACTIVITY_TYPE_POSTS) {
                return {
                    ...state,
                    activityType: action.payload.activityType,
                    activityList: action.payload.activityList,
                };
            } else if (action.payload.activityType === ACTIVITY_TYPE_COMMENTS) {
                return {
                    ...state,
                    activityType: action.payload.activityType,
                    activityList: action.payload.activityList,
                };
            }
            break;
        case "addActivityByScroll":
            if (state.activityType === ACTIVITY_TYPE_POSTS) {
                return {
                    ...state,
                    activityList: [...state.activityList, ...action.payload.activityList],
                };
            } else if (state.activityType === ACTIVITY_TYPE_COMMENTS) {
                return {
                    ...state,
                    activityList: [...state.activityList, ...action.payload.activityList],
                };
            }
            break;
        case "updateUserId":
            return {
                ...state,
                userId: action.payload.userId,
            };
        case "increasePageCount":
            return {
                ...state,
                page: state.page + 1,
            };
        default:
            return state;
    }
};

export const AccountActivity = () => {
    const navigate = useNavigate();

    const [state, setState] = useReducer(reducer, {
        activityType: ACTIVITY_TYPE_POSTS,
        activityList: [],
        page: 0,
        userId: "",
    });

    const { JWT, removeAuth } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    useEffect(() => {
        getUserDataByJWT(JWT)
            .then((result) => {
                setState({
                    type: "updateUserId",
                    payload: {
                        userId: result.guid,
                    },
                });
                loadPostsPageByUserId(JWT, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT).then((result) => {
                    setState({
                        type: "changeActivityType",
                        payload: {
                            activityType: state.activityType,
                            activityList: result,
                        },
                    });
                });
            })
            .catch((err) => {
                if (JWT && err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                    setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                    removeAuth();
                    navigate("/login");
                } else {
                    setMessageBoxSettings("You should login before creating a post!", false);
                    navigate("/login");
                }
            });
    }, [JWT, removeAuth, navigate, setMessageBoxSettings, state.activityType]);

    const handleActivityTypeChange = (e) => {
        if (e.target.value === ACTIVITY_TYPE_POSTS) {
            loadPostsPageByUserId(JWT, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT).then((result) => {
                setState({
                    type: "changeActivityType",
                    payload: {
                        activityType: ACTIVITY_TYPE_POSTS,
                        activityList: result,
                    },
                });
            });
        } else if (e.target.value === ACTIVITY_TYPE_COMMENTS) {
            getCommentsByUserId(JWT, 0, COMMENTS_LIST_IN_POST_PAGE_COUNT).then((result) => {
                setState({
                    type: "changeActivityType",
                    payload: {
                        activityType: ACTIVITY_TYPE_COMMENTS,
                        activityList: result,
                    },
                });
            });
        }
    };

    const loadComments = useCallback(
        (page, pageCount) => {
            getCommentsByUserId(JWT, page, pageCount).then((result) => {
                setState({
                    type: "addActivityByScroll",
                    payload: {
                        activityList: result,
                    },
                });
            });
        },
        [JWT]
    );

    const loadPosts = useCallback(
        (page, pageCount) => {
            loadPostsPageByUserId(JWT, page, pageCount).then((result) => {
                setState({
                    type: "addActivityByScroll",
                    payload: {
                        activityList: result,
                    },
                });
            });
        },
        [JWT]
    );

    const handleScroll = useCallback(
        (e) => {
            const scrollHeight = e.target.documentElement.scrollHeight;
            const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

            if (currentHeight >= scrollHeight - 1 || currentHeight === scrollHeight) {
                setState({
                    type: "increasePageCount",
                });
                if (state.activityType === ACTIVITY_TYPE_POSTS) {
                    loadPosts(state.page + 1, POSTS_LIST_POSTS_IN_PAGE_COUNT);
                } else if (state.activityType === ACTIVITY_TYPE_COMMENTS) {
                    loadComments(state.page + 1, COMMENTS_LIST_IN_POST_PAGE_COUNT);
                }
            }
        },
        [loadPosts, loadComments, state.page, state.activityType]
    );

    useEffect(() => {
        window.addEventListener("scroll", handleScroll);

        return () => window.removeEventListener("scroll", handleScroll);
    }, [handleScroll]);

    return (
        <div className={styles.ActivityContainer}>
            <div className={styles.gotoTopButtonContainer}></div>
            <select
                onChange={handleActivityTypeChange}
                value={state.activityType}
                className={styles.activityTypeSelectorType}
            >
                <option value="posts">Posts</option>
                <option value="comments">Comments</option>
            </select>

            <section className={styles.listContainer}>
                {state.activityType === ACTIVITY_TYPE_POSTS ? (
                    <PostsList posts={state.activityList} />
                ) : (
                    <CommentsList comments={state.activityList} />
                )}
            </section>
        </div>
    );
};
