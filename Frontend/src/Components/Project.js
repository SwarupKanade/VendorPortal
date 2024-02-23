import React from 'react';
import './Style.css'; 

const Project = () => {
    return (
        <div className="main">
            <div className="container">
                <h1>Project Details</h1>
                <form>
                    <table>
                        <tbody>
                            <tr>
                                <td className="td-class">
                                    <label htmlFor="projectName">Project Name:</label>
                                </td>
                                <td>
                                    <input className="input-class" type="text" />
                                </td>
                            </tr>
                            <tr>
                                <td className="td-class">
                                    <label htmlFor="projectHeadName">Project Head Name:</label>
                                </td>
                                <td>
                                    <input className="input-class" type="text" />
                                </td>
                            </tr>
                            <tr>
                                <td className="td-class">
                                    <label htmlFor="projectStatus">Project Status:</label>
                                </td>
                                <td>
                                    <div>
                                        <input className="input-class" type="radio" id="active" name="status" />
                                        <label htmlFor="active">Active</label>
                                    </div>
                                    <div>
                                        <input className="input-class" type="radio" id="inactive" name="status" />
                                        <label htmlFor="inactive">Inactive</label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td className="td-class">
                                    <label htmlFor="date">Date:</label>
                                </td>
                                <td>
                                    <input className="input-class" type="date" />
                                </td>
                            </tr>
                            <tr>
                                <td className="td-class">
                                    <label htmlFor="description">Description:</label>
                                </td>
                                <td>
                                    <input className="input-class" type="text" />
                                </td>
                            </tr>
                            <tr>
                                <td className="td-class" colSpan="2">
                                    <button className="button" type="submit">Save</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </form>
            </div>
        </div>
    );
}

export default Project;