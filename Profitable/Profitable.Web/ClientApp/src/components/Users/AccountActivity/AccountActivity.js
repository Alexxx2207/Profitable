import { useEffect, useReducer, useCallback } from "react";
import {
    ACTIVITY_TYPE_COMMENTS,
    ACTIVITY_TYPE_POSTS,
    COMMENTS_LIST_IN_POST_PAGE_COUNT,
    POSTS_LIST_POSTS_IN_PAGE_COUNT,
} from "../../../common/config";
import { loadPostsPageByUserId } from "../../../services/posts/postsService";
import { CommentsList } from "../../PostsAndComments/Comments/CommentsList/CommentsList";
import { PostsList } from "../../PostsAndComments/Posts/PostsList/PostsList";
import { getCommentsByUserId } from "../../../services/comments/commentsService";

import styles from "./AccountActivity.module.css";
import { useParams } from "react-router-dom";

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
    const [state, setState] = useReducer(reducer, {
        activityType: ACTIVITY_TYPE_POSTS,
        activityList: [],
        page: 0,
    });

    const { searchedProfileEmail } = useParams();

    useEffect(() => {
        loadPostsPageByUserId(searchedProfileEmail, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT).then(
            (result) => {
                console.log("a");
                setState({
                    type: "changeActivityType",
                    payload: {
                        activityType: state.activityType,
                        activityList: result,
                    },
                });
            }
        );
    }, [state.activityType, searchedProfileEmail]);

    const handleActivityTypeChange = (e) => {
        if (e.target.value === ACTIVITY_TYPE_POSTS) {
            loadPostsPageByUserId(searchedProfileEmail, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT).then(
                (result) => {
                    setState({
                        type: "changeActivityType",
                        payload: {
                            activityType: ACTIVITY_TYPE_POSTS,
                            activityList: result,
                        },
                    });
                }
            );
        } else if (e.target.value === ACTIVITY_TYPE_COMMENTS) {
            getCommentsByUserId(searchedProfileEmail, 0, COMMENTS_LIST_IN_POST_PAGE_COUNT).then(
                (result) => {
                    setState({
                        type: "changeActivityType",
                        payload: {
                            activityType: ACTIVITY_TYPE_COMMENTS,
                            activityList: result,
                        },
                    });
                }
            );
        }
    };

    const loadComments = useCallback(
        (page, pageCount) => {
            getCommentsByUserId(searchedProfileEmail, page, pageCount).then((result) => {
                setState({
                    type: "addActivityByScroll",
                    payload: {
                        activityList: result,
                    },
                });
            });
        },
        [searchedProfileEmail]
    );

    const loadPosts = useCallback(
        (page, pageCount) => {
            loadPostsPageByUserId(searchedProfileEmail, page, pageCount).then((result) => {
                setState({
                    type: "addActivityByScroll",
                    payload: {
                        activityList: result,
                    },
                });
            });
        },
        [searchedProfileEmail]
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
            <select onChange={handleActivityTypeChange} className={styles.activityTypeSelectorType}>
                <option value={ACTIVITY_TYPE_POSTS}>Posts</option>
                <option value={ACTIVITY_TYPE_COMMENTS}>Comments</option>
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
