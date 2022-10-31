import { useContext, useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { AuthContext } from "../../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../../contexts/MessageBoxContext";
import { ErrorWidget } from "../../../../Common/ErrorWidget/ErrorWidget";

import {
    CLIENT_ERROR_TYPE,
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    LongDirectionName,
    SERVER_ERROR_TYPE,
    ShortDirectionName,
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

import {
    changePosition,
    getPositionByGuid,
} from "../../../../../services/positions/positionsService";
import { loadFuturesContracts } from "../../../../../services/futures/futuresService";
import { getUserEmailFromJWT } from "../../../../../services/users/usersService";

import styles from "./ChangeFuturesPosition.module.css";

const reducer = (state, action) => {
    switch (action.type) {
        case "changeSelectedDirection":
            return {
                ...state,
                values: {
                    ...state.values,
                    directionSelected: action.payload,
                },
            };
        case "loadAllFuturesContracts":
            return {
                ...state,
                values: {
                    ...state.values,
                    allContracts: action.payload,
                },
            };
        case "selectContract":
            return {
                ...state,
                values: {
                    ...state.values,
                    chosenContract: state.values.allContracts[action.payload],
                },
            };
        case "loadPosition":
            return {
                ...state,
                values: {
                    ...state.values,
                    chosenContract: {
                        name: action.payload.contractName,
                        tickSize: action.payload.tickSize,
                        tickValue: action.payload.tickValue,
                    },
                    directionSelected:
                        LongDirectionName.localeCompare(action.payload.direction) === 0
                            ? LongDirectionName
                            : ShortDirectionName,
                    entryPrice: action.payload.entryPrice,
                    exitPrice: action.payload.exitPrice,
                    quantity: action.payload.quantity,
                },
                errors: {
                    ...state.errors,

                    entryPriceEmpty: createClientErrorObject(
                        state.errors.entryPriceEmpty,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.entryPrice)
                    ),
                    exitPriceEmpty: createClientErrorObject(
                        state.errors.exitPriceEmpty,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.exitPrice)
                    ),
                    quantityEmpty: createClientErrorObject(
                        state.errors.quantityEmpty,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.quantity)
                    ),
                    quantityOverZero: createClientErrorObject(
                        state.errors.quantityOverZero,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.quantity)
                    ),
                },
            };
        case "setContractsCount":
            if (action.payload >= 0) {
                return {
                    ...state,
                    values: {
                        ...state.values,
                        quantity: action.payload,
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
                        quantity: 0,
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

        default:
            break;
    }
};

export const ChangeFuturesPosition = () => {
    const { recordGuid, searchedProfileEmail, positionGuid } = useParams();

    const navigate = useNavigate();

    const { JWT, removeAuth } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(reducer, {
        values: {
            allContracts: [],
            chosenContract: {
                name: "",
                tickSize: 0,
                tickValue: 0,
            },
            directionSelected: LongDirectionName,
            entryPrice: "",
            exitPrice: "",
            quantity: "",
        },
        errors: {
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
                text: "Insert Contracts Quantity",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            quantityOverZero: {
                text: "Contracts Quantity > 0",
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

        loadFuturesContracts().then((futuresContracts) =>
            setState({
                type: "loadAllFuturesContracts",
                payload: futuresContracts,
            })
        );

        getPositionByGuid(recordGuid, positionGuid).then((position) =>
            setState({
                type: "loadPosition",
                payload: position,
            })
        );
        // eslint-disable-next-line
    }, []);

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            changePosition(
                JWT,
                recordGuid,
                positionGuid,
                state.values.chosenContract.name,
                state.values.directionSelected,
                state.values.entryPrice,
                state.values.exitPrice,
                state.values.quantity,
                state.values.chosenContract.tickSize,
                state.values.chosenContract.tickValue
            )
                .then((jwt) => {
                    setMessageBoxSettings("The position was edited successfully!", true);
                    navigate(
                        `/users/${searchedProfileEmail}/positions-records/futures/${recordGuid}`
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
                    } else {
                        setState((state) => ({
                            ...state,
                            errors: {
                                ...state.errors,
                                serverError: createServerErrorObject(err.message, true),
                            },
                        }));
                    }
                });
        }
    };

    const OnChangeContractType = (value) => {
        setState({
            type: "selectContract",
            payload: value,
        });
    };

    const onBullishDirectionButtonClick = (e) => {
        setState({
            type: "changeSelectedDirection",
            payload: LongDirectionName,
        });
    };

    const onBearishDirectionButtonClick = (e) => {
        setState({
            type: "changeSelectedDirection",
            payload: ShortDirectionName,
        });
    };

    const changeContractsCountHandler = (e) => {
        setState({
            type: "setContractsCount",
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
            <div className={styles.changeSectionContainer}>
                <div className={styles.changeContainer}>
                    <form className={styles.changeForm} onSubmit={onSubmit}>
                        <div className={styles.changeLabelContainer}>
                            <h2 className={styles.changeLabel}>Change Position</h2>
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Select Contract</h5>
                            </div>

                            <select
                                className={styles.selectContract}
                                onChange={(e) => OnChangeContractType(e.target.value)}
                                value={
                                    state.values.allContracts.find(
                                        (el) => el.name === state.values.chosenContract.name
                                    )?.name
                                }
                            >
                                {state.values.allContracts.map((futureContract, index) => (
                                    <option
                                        className={styles.selectContractOption}
                                        key={futureContract.guid}
                                        value={futureContract.name}
                                    >
                                        {futureContract.name}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className={styles.formGroup}>
                            <div>
                                <h5>Position Direction</h5>
                            </div>

                            <section className={styles.directions}>
                                <div
                                    className={styles.radioWrapper}
                                    onClick={(e) => onBullishDirectionButtonClick(e)}
                                >
                                    <input
                                        type="radio"
                                        onChange={(e) => onBullishDirectionButtonClick(e)}
                                        checked={
                                            state.values.directionSelected.localeCompare(
                                                LongDirectionName
                                            ) === 0
                                        }
                                        className={styles.directionBullish}
                                        name="directionSelected"
                                    />
                                    <span
                                        className={styles.radioLabel}
                                        onClick={(e) => onBullishDirectionButtonClick(e)}
                                    >
                                        Bullish
                                    </span>
                                </div>
                                <div
                                    className={styles.radioWrapper}
                                    onClick={(e) => onBearishDirectionButtonClick(e)}
                                >
                                    <input
                                        type="radio"
                                        onChange={(e) => onBearishDirectionButtonClick(e)}
                                        checked={
                                            state.values.directionSelected.localeCompare(
                                                ShortDirectionName
                                            ) === 0
                                        }
                                        className={styles.directionBearish}
                                        name="directionSelected"
                                    />
                                    <span
                                        className={styles.radioLabel}
                                        onClick={(e) => onBearishDirectionButtonClick(e)}
                                    >
                                        Bearish
                                    </span>
                                </div>
                            </section>
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
                                name="quantity"
                                value={state.values.quantity}
                                onChange={changeContractsCountHandler}
                                type="number"
                                step=".001"
                            />
                        </div>

                        <div className={styles.submitButtonContainer}>
                            <input className={styles.submitButton} type="submit" value="Change" />
                        </div>
                    </form>
                </div>
                <aside className={styles.changeAside}>
                    <div className={styles.errorsHeadingContainer}>
                        <h2 className={styles.errorsHeading}>Change State</h2>
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
