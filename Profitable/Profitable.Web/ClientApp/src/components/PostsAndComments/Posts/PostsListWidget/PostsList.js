import { useState, useEffect, useCallback } from 'react';
import { PostsListItem } from '../PostsListItem/PostsListItem';
import styles from './PostsList.module.css';

export const PostsList = () => {
    const [posts, setPosts] = useState([]);
    let page = 0;

    const loadPosts = useCallback(() => {
        fetch(`${process.env.REACT_APP_API_BASE_URL}/api/posts/${page}`)
            .then(response => response.json())
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

    useEffect(() => {
        loadPosts();
        window.addEventListener("scroll", handleScroll);
    }, [handleScroll, loadPosts]);

    return (
        <div className={styles.wrapper}>
            <h1 className={styles.discoverHeading}>Discover</h1>
            <div className={styles.postsList}>
                {posts.map(post => <PostsListItem key={post.guid} {...post} />)}
            </div>
        </div>
    );
}