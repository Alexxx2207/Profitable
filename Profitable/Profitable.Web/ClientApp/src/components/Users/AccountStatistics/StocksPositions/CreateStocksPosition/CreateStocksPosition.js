import { useContext, useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { AuthContext } from "../../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../../contexts/MessageBoxContext";
import { ErrorWidget } from "../../../../Common/ErrorWidget/ErrorWidget";

import {
    CLIENT_ERROR_TYPE,
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    REQUESTER_NOT_OWNER_FRIENDLIER_MESSAGE,
    REQUESTER_NOT_OWNER_MESSAGE,
    SERVER_ERROR_TYPE,
} from "../../../../../common/config";
import {
    isEmptyOrWhiteSpaceFieldChecker,
    naturalNumberChecker,
} from "../../../../../services/common/errorValidationCheckers";
import { changeStateValuesForControlledForms } from "../../../../../services/common/createStateValues";
import {
    createClientErrorObject,
    createServerErrorObject,
} from "../../../../../services/common/createValidationErrorObject";

import { createPosition } from "../../../../../services/positions/stocksPositionsService";
import { getUserEmailFromJWT } from "../../../../../services/users/usersService";

import styles from "./CreateStocksPosition.module.css";

const reducer = (state, action) => {
    switch (action.type) {
        case "setSharesCount":
            if (action.payload >= 0) {
                return {
                    ...state,
                    values: {
                        ...state.values,
                        quantitySize: action.payload,
                    },
                    errors: {
                        ...state.errors,
                        quantityEmpty: createClientErrorObject(
                            state.errors.quantityEmpty,
                            isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload)
                        ),
                        quantityOverZero: createClientErrorObject(
                            state.errors.quantityOverZero,
                            naturalNumberChecker.bind(null, action.payload)
                        ),
                    },
                };
            } else {
                return {
                    ...state,
                    values: {
                        ...state.values,
                        quantitySize: 0,
                    },
                    errors: {
                        ...state.errors,
                        quantityEmpty: createClientErrorObject(
                            state.errors.quantityEmpty,
                            isEmptyOrWhiteSpaceFieldChecker.bind(null, "0")
                        ),
                        quantityOverZero: createClientErrorObject(
                            state.errors.quantityOverZero,
                            naturalNumberChecker.bind(null, "0")
                        ),
                    },
                };
            }
        case "updateInput":
            return {
                ...state,
                values: changeStateValuesForControlledForms(
                    state.values,
                    action.payload.name,
                    action.payload.value
                ),
                errors: {
                    ...state.errors,
                    [action.payload.name + "Empty"]: createClientErrorObject(
                        state.errors[action.payload.name + "Empty"],
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.value)
                    ),
                },
            };
        case "setServerError":
            return {
                ...state,
                errors: {
                    ...state.errors,
                    serverError: createServerErrorObject(action.payload, true),
                },
            };
        default:
            return {
                ...state,
            };
    }
};

export const CreateStocksPosition = () => {
    const { recordGuid, searchedProfileEmail } = useParams();

    const navigate = useNavigate();

    const { JWT, removeAuth } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(reducer, {
        values: {
            name: "",
            entryPrice: "",
            exitPrice: "",
            quantitySize: "",
            buyCommission: "",
            sellCommission: "",
        },
        errors: {
            nameEmpty: {
                text: "Insert Name",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            entryPriceEmpty: {
                text: "Insert Entry Price",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            exitPriceEmpty: {
                text: "Insert Exit Price",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            quantityEmpty: {
                text: "Insert Shares Quantity",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            quantityOverZero: {
                text: "Shares Quantity > 0",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            buyCommissionEmpty: {
                text: "Insert Buy Commission",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            sellCommissionEmpty: {
                text: "Insert Sell Commission",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            serverError: {
                text: "",
                display: false,
                type: SERVER_ERROR_TYPE,
            },
        },
    });

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then((result) => result)
            .catch((err) => {
                setMessageBoxSettings("You should login before creating a position!", false);
                removeAuth();
                navigate("/login");
            });
        // eslint-disable-next-line
    }, []);

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            createPosition(
                JWT,
                recordGuid,
                state.values.name,
                state.values.entryPrice,
                state.values.exitPrice,
                state.values.quantitySize,
                state.values.buyCommission,
                state.values.sellCommission
            )
                .then(() => {
                    setMessageBoxSettings("The position was edited successfully!", true);
                    navigate(
                        `/users/${searchedProfileEmail}/positions-records/stocks/${recordGuid}`
                    );
                })
                .catch((err) => {
                    if (JWT && err.message === "Should auth first") {
                        setMessageBoxSettings(
                            JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE +
                                " The position was not edited!",
                            false
                        );
                        removeAuth();
                        navigate("/login");
                    } else if (JWT && err.message === REQUESTER_NOT_OWNER_MESSAGE) {
                        setState({
                            type: "setServerError",
                            payload: REQUESTER_NOT_OWNER_FRIENDLIER_MESSAGE,
                        });
                    } else {
                        setState({
                            type: "setServerError",
                            payload: err.message,
                        });
                    }
                });
        }
    };

    const changeSharesCountHandler = (e) => {
        setState({
            type: "setSharesCount",
            payload: e.target.value,
        });
    };

    const changeHandler = (e) => {
        setState({
            type: "updateInput",
            payload: e.target,
        });
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.createSectionContainer}>
                <div className={styles.createContainer}>
                    <form className={styles.createForm} onSubmit={onSubmit}>
                        <div className={styles.createLabelContainer}>
                            <h2 className={styles.createLabel}>Create Position</h2>
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Stock Name</h5>
                            </div>

                            <input
                                className={styles.inputField}
                                type="text"
                                name="name"
                                placeholder={"AAPL/GOOG/NVDA..."}
                                value={state.values.title}
                                onChange={changeHandler}
                            />
                        </div>

                        <div className={styles.formGroup}>
                            <div>
                                <h5>Entry Price</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                name="entryPrice"
                                value={state.values.entryPrice}
                                onChange={changeHandler}
                                type="number"
                                step=".000001"
                            />
                        </div>

                        <div className={styles.formGroup}>
                            <div>
                                <h5>Exit Price</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                name="exitPrice"
                                value={state.values.exitPrice}
                                onChange={changeHandler}
                                type="number"
                                step=".000001"
                            />
                        </div>

                        <div className={styles.formGroup}>
                            <div>
                                <h5>Quantity</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                name="quantitySize"
                                value={state.values.quantitySize}
                                onChange={changeSharesCountHandler}
                                type="number"
                                step=".001"
                            />
                        </div>

                        <div className={styles.formGroup}>
                            <div>
                                <h5>Buy Commission</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                name="buyCommission"
                                value={state.values.buyCommission}
                                onChange={changeHandler}
                                type="number"
                                step=".000001"
                            />
                        </div>

                        <div className={styles.formGroup}>
                            <div>
                                <h5>Sell Commission</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                name="sellCommission"
                                value={state.values.sellCommission}
                                onChange={changeHandler}
                                type="number"
                                step=".000001"
                            />
                        </div>

                        <div className={styles.submitButtonContainer}>
                            <input className={styles.submitButton} type="submit" value="Create" />
                        </div>
                    </form>
                </div>
                <aside className={styles.createAside}>
                    <div className={styles.errorsHeadingContainer}>
                        <h2 className={styles.errorsHeading}>Create State</h2>
                    </div>
                    <div className={styles.errorsContainer}>
                        {Object.values(state.errors).map((error, index) => (
                            <ErrorWidget key={index} error={error} />
                        ))}
                    </div>
                </aside>
            </div>
        </div>
    );
};
