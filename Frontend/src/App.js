import React, { useState, useEffect } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import axios from "axios";

import Login from "./pages/Login";
import VendorDashboard from "./pages/VendorDashboard";
import AdminDashboard from "./pages/AdminDashboard";
import ProjectHeadDashboard from "./pages/ProjectHeadDashboard";

//Admin
import AdminDash from "./pages/admin/AdminDash";
import Users from "./pages/admin/Users";
import AddVendor from "./pages/admin/AddVendor";
import AddProjectHead from "./pages/admin/AddProjectHead";
import AddVendorCategory from "./pages/admin/AddVendorCategory";
import VendorCategory from "./pages/admin/VendorCategory";
import AddProject from "./pages/admin/AddProject";
import Project from "./pages/admin/Project";
import RFPA from "./pages/admin/RFP";
import AddRFP from "./pages/admin/AddRFP";

//Vendor
import VendorDash from "./pages/vendor/VendorDash";
import RFP from "./pages/vendor/RFP";
import VendorProfile from "./pages/vendor/VendorProfile";

//ProjectHead
import ProjectHeadDash from "./pages/projecthead/ProjectHeadDash";
import ProjectHeadProfile from "./pages/projecthead/ProjectHeadProfile";
import AssignedProject from "./pages/projecthead/AssignedProject";

const App = () => {
  const [userRole, setuserRole] = useState(sessionStorage.getItem("roles"));
  useEffect(() => {
    console.log(userRole);
  }, []);

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />

        <Route path="/admin" element={<AdminDashboard />}>
          <Route index element={<AdminDash />} />
          <Route path="dashboard" index element={<AdminDash />} />
          <Route path="allusers" element={<Users />} />
          <Route path="create-vendor" element={<AddVendor />} />
          <Route path="create-project-head" element={<AddProjectHead />} />
          <Route path="add-vendor-category" element={<AddVendorCategory />} />
          <Route path="vendor-category" element={<VendorCategory />} />
          <Route path="projects" element={<Project />} />
          <Route path="create-project" element={<AddProject />} />
          <Route path="rfp" element={<RFPA />} />
          <Route path="create-rfp" element={<AddRFP />} />
        </Route>

        <Route path="/vendor" element={<VendorDashboard />}>
          <Route index element={<VendorDash />} />
          <Route path="dashboard" index element={<VendorDash />} />
          <Route path="profile" element={<VendorProfile />} />
          <Route path="rfp" index element={<RFP />} />
        </Route>

        <Route path="/projecthead" element={<ProjectHeadDashboard />}>
          <Route index element={<ProjectHeadDash />} />
          <Route path="dashboard" index element={<ProjectHeadDash />} />
          <Route path="profile" element={<ProjectHeadProfile />} />
          <Route path="assigned-project" element={<AssignedProject />} />
        </Route>

        <Route path="/" element={<Navigate to="/login" />} />
      </Routes>
    </Router>
  );
};

export default App;
