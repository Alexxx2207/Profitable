import { useEffect, useReducer, useCallback, useContext } from "react";
import { useParams } from "react-router-dom";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";
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

const reducer = (state, action) => {
    switch (action.type) {
        case "changeActivityType":
            return {
                ...state,
                page: 0,
                showShowMore: true,
                activityType: action.payload.activityType,
                activityList: action.payload.activityList,
            };
        case "addActivity":
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
        case "hideShowMoreButton":
            return {
                ...state,
                showShowMore: action.payload,
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
        showShowMore: true,
    });

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { searchedProfileEmail } = useParams();

    useEffect(() => {
        loadPostsPageByUserId(searchedProfileEmail, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT).then(
            (result) => {
                if (result.length > 0) {
                    setState({
                        type: "changeActivityType",
                        payload: {
                            activityType: state.activityType,
                            activityList: result,
                        },
                    });
                } else {
                    setState({
                        type: "hideShowMoreButton",
                        payload: false,
                    });
                }
            }
        );
    }, [searchedProfileEmail]);

    const handleActivityTypeChange = (e) => {
        if (e.target.value === ACTIVITY_TYPE_POSTS) {
            loadPostsPageByUserId(searchedProfileEmail, 0, POSTS_LIST_POSTS_IN_PAGE_COUNT).then(
                (result) => {
                    if (result.length > 0) {
                        setState({
                            type: "changeActivityType",
                            payload: {
                                activityType: ACTIVITY_TYPE_POSTS,
                                activityList: result,
                            },
                        });
                    } else {
                        setState({
                            type: "hideShowMoreButton",
                            payload: false,
                        });
                    }
                }
            );
        } else if (e.target.value === ACTIVITY_TYPE_COMMENTS) {
            getCommentsByUserId(searchedProfileEmail, 0, COMMENTS_LIST_IN_POST_PAGE_COUNT).then(
                (result) => {
                    if (result.length > 0) {
                        setState({
                            type: "changeActivityType",
                            payload: {
                                activityType: ACTIVITY_TYPE_COMMENTS,
                                activityList: result,
                            },
                        });
                    } else {
                        setState({
                            type: "hideShowMoreButton",
                            payload: false,
                        });
                    }
                }
            );
        }
    };

    const loadComments = useCallback(
        (page, pageCount) => {
            getCommentsByUserId(searchedProfileEmail, page, pageCount).then((result) => {
                if (result.length > 0) {
                    setState({
                        type: "addActivity",
                        payload: {
                            activityList: result,
                        },
                    });
                } else {
                    setMessageBoxSettings("There are no more comments", false);
                    setState({
                        type: "hideShowMoreButton",
                        payload: false,
                    });
                }
            });
        },
        [searchedProfileEmail]
    );

    const loadPosts = useCallback(
        (page, pageCount) => {
            loadPostsPageByUserId(searchedProfileEmail, page, pageCount).then((result) => {
                if (result.length > 0) {
                    setState({
                        type: "addActivity",
                        payload: {
                            activityList: result,
                        },
                    });
                } else {
                    setMessageBoxSettings("There are no more posts", false);
                    setState({
                        type: "hideShowMoreButton",
                        payload: false,
                    });
                }
            });
        },
        [searchedProfileEmail]
    );

    const handleShowMorePostsClick = useCallback(
        (e) => {
            e.preventDefault();
            setState({
                type: "increasePageCount",
            });
            if (state.activityType === ACTIVITY_TYPE_POSTS) {
                loadPosts(state.page + 1, POSTS_LIST_POSTS_IN_PAGE_COUNT);
            } else if (state.activityType === ACTIVITY_TYPE_COMMENTS) {
                loadComments(state.page + 1, COMMENTS_LIST_IN_POST_PAGE_COUNT);
            }
        },
        [loadPosts, loadComments, state.page, state.activityType]
    );

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
            {state.showShowMore ? (
                <div className={styles.loadMoreContainer}>
                    <h4 className={styles.loadMoreButton} onClick={handleShowMorePostsClick}>
                        Show More{" "}
                        {state.activityType[0].toUpperCase() + state.activityType.slice(1)}
                    </h4>
                </div>
            ) : (
                <></>
            )}
        </div>
    );
};
