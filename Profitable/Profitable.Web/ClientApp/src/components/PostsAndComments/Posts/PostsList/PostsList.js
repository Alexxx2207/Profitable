import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { PostsListItem } from './PostsListItem/PostsListItem';

import { loadPostsPage } from '../../../../services/posts/postsService';

import { POSTS_LIST_POSTS_IN_PAGE_COUNT } from '../../../../common/config';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import styles from './PostsList.module.css';

export const PostsList = () => {

    const navigate = useNavigate();

    const [posts, setPosts] = useState([]);
    let page = 0;

    

    const loadPosts = useCallback(() => {
        loadPostsPage(page, POSTS_LIST_POSTS_IN_PAGE_COUNT)
            .then(result => {
                if (result.length > 0) {
                    setPosts(posts => [...posts, ...result])
                }
            });
        page++;
    }, [page]);

    const handleScroll = useCallback((e) => {
        const scrollHeight = e.target.documentElement.scrollHeight;
        const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

        if (currentHeight >= scrollHeight - 1 || currentHeight === scrollHeight) {
            loadPosts();
        }
    }, [loadPosts]);

    const addPostClickHandler = () => {
        setPosts([]);
        navigate('/posts/create');
    };

    useEffect(() => {
        loadPosts();
        window.addEventListener("scroll", handleScroll);

        return () => window.removeEventListener('scroll', handleScroll);
    }, [handleScroll, loadPosts]);

    return (
        <div className={styles.wrapper}>
            <div className={styles.createPostContainer}>
            <button className={styles.addPostButton} onClick={addPostClickHandler}>
                    <FontAwesomeIcon className={styles.iconPlus} icon={faCirclePlus} />
                    <div className={styles.addPostText}>
                        Add Post
                    </div>
                </button>
            </div>
            <h1 className={styles.discoverHeading}>Discover</h1>
            <div className={styles.postsList}>
                {posts.map(post => <PostsListItem key={post.guid} {...post} />)}
            </div>
        </div>
    );
}