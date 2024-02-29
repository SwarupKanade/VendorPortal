import React, { useState } from "react";
import Sidebar from "../components/sidebar";
import Navbar from "../components/navbar";
import { Outlet } from "react-router-dom";

export default function AdminDashboard() {
  const [isMenuVisible, setMenuVisible] = useState(false);

  const menuItems = [
    {
      text: "Dashboard",
      icon: "ri-home-2-line",
      link: "dashboard",
    },
    {
      text: "Users",
      icon: "bx bx-user",
      link: "#",
      subItems: [{ text: "All", link: "allusers" }],
    },
    {
      text: "Vendor",
      icon: "bx bx-category",
      link: "#",
      subItems: [
        { text: "Create Vendor", link: "create-vendor" },
        { text: "All Vendor Category", link: "vendor-category" },
        { text: "Add Vendor Category", link: "add-vendor-category" },
      ],
    },
    {
      text: "Project Head",
      icon: "bx bx-user",
      link: "#",
      subItems: [{ text: "Create Project Head", link: "create-project-head" }],
    },
    {
      text: "Project",
      icon: "ri ri-projector-line",
      link: "#",
      subItems: [
        { text: "All Project", link: "projects" },
        { text: "Create Project", link: "create-project" },
      ],
    },
    {
      text: "Request for Proposal",
      icon: "ri ri-file-2-line-2",
      link: "#",
      subItems: [
        { text: "All RFP", link: "rfp" },
        { text: "Create RFP", link: "create-rfp" },
      ],
    },
  ];

  const handleMenuVisible = () => {
    setMenuVisible(!isMenuVisible);
  };
  return (
    <>
      <Sidebar
        isMenuVisible={isMenuVisible}
        handleMenuVisible={handleMenuVisible}
        menuItems={menuItems}
      />
      <main class="w-full md:w-[calc(100%-256px)] sm:ml-0 md:ml-64 bg-gray-200 min-h-screen transition-all main">
        <Navbar handleMenuVisible={handleMenuVisible} />
        <div class="p-6">
          <Outlet />
        </div>
      </main>
    </>
  );
}
