import { useContext, useState } from "react";
import { organizationRolesToManage } from "../../../../common/config";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { createAuthorImgURL } from "../../../../services/common/imageService";
import { addMemberFromOrganization, removeMemberFromOrganization } from "../../../../services/organization/organizationsService";
import { editUserRole } from "../../../../services/users/usersService";
import styles from "./MemberSearchResult.module.css";

export const MemberSearchResult = ({
    JWT, user, organizationId, userManager }) => {

    const [state, setState] = useState({...user});

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const clickAddRemoveHandler = (e) => {
        e.preventDefault();
        e.stopPropagation();
        // eslint-disable-next-line no-lone-blocks
        if(state.organizationId) {
            removeMemberFromOrganization(JWT, user.guid)
            .then(result => {
                setMessageBoxSettings(
                    `User was removed successfully`,
                    true
                );
                setState((old) => ({
                    ...state, 
                    organizationId: null,
                    organizationRole: organizationRolesToManage.None
                }));
            })
            .catch(err => {
                setMessageBoxSettings(
                    `User was not removed successfully`,
                    false
                );
            });
        } else {
            addMemberFromOrganization(JWT, organizationId, user.guid)
                .then(result => {
                    setMessageBoxSettings(
                        `User was added successfully`,
                        true
                    );
                    setState((old) => ({
                        ...state, 
                        organizationId: organizationId,
                        organizationRole: organizationRolesToManage.Member
                    }));
                })
                .catch(err => {
                    setMessageBoxSettings(
                        `User was not added successfully`,
                        false
                    );
                });
        }
    };

    const onRoleSelectorChange = (e) => {
        editUserRole(JWT, user.guid, e.target.value)
        .then(result => {
            setMessageBoxSettings(
                `User role was changed successfully`,
                true
            );
            setState((old) => ({
                ...state, 
                organizationRole: result
            }));
        })
        .catch(err => {
            setMessageBoxSettings(
                `User role was not changed successfully`,
                false
            );
        });
    };

    return (
        <div className={styles.userContainer}>
            <div className={styles.useInfoContainer}>
                <img
                    className={styles.authorImage}
                    src={createAuthorImgURL(user.profileImage)}
                    alt=""
                />
                <div>
                    <h4>
                        {user.firstName} {user.lastName}
                    </h4>
                    <h6>{user.email}</h6>
                    <p className={styles.description}>{user.description}</p>
                </div>
            </div>
            <div className={styles.managementContainer}>
                {!state.organizationId ? 
                    <button 
                        className={styles.addButton}
                        onClick={clickAddRemoveHandler}>
                        "Add"
                    </button>
                :
                    state.organizationId === userManager.organizationId &&
                        state.organizationRole.localeCompare(organizationRolesToManage.Owner) !== 0 ?
                        <>
                            <select 
                                className={styles.roleSelector}
                                value={state.organizationRole}
                                onChange={onRoleSelectorChange}>
                                <option value={organizationRolesToManage.Admin}>
                                    {organizationRolesToManage.Admin}
                                </option>
                                <option value={organizationRolesToManage.Member}>
                                    {organizationRolesToManage.Member}
                                </option>
                            </select>
                            <button 
                                className={styles.addButton}
                                onClick={clickAddRemoveHandler}>
                                "Remove"
                            </button>
                        </> 
                    :
                    <></>
                }
                
            </div>
        </div>
    );
};
