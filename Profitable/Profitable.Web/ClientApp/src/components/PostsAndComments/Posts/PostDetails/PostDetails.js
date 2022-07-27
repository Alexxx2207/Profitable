import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { createImgURL, loadParticularPost } from '../../../../services/posts/postsService';
import styles from './PostDetails.module.css';

export const PostDetails = () => {

    const { postId } = useParams();
    const [ post, setPost ] = useState({
        title: '',
        content: '',
        author: '',
        postedOn: '',
        imageType: '',
        image: '',
        likes: [],
        comments: [],
    });

    useEffect(() => {
        loadParticularPost(postId)
            .then(result => setPost(result))
    }, [postId])

    return (
    <div className={styles.post}>
        <img src={createImgURL(post)} alt="" />
        <div className={styles.text}>
            <h1>{post.title}</h1>
            <p>{post.content}</p>
        </div>
        <div className={styles.information}>
            <div className={styles.author}>
                {post.author}
            </div>
            <div className={styles.postedOn}>
                {post.postedOn}
            </div>
        </div>
    </div>);
}