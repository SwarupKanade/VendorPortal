import React,{useState, useEffect} from "react";
import { Link } from "react-router-dom";

export default function Sidebar({
  isMenuVisible,
  handleMenuVisible,
  menuItems,
}) {
  
  const [openMenus, setOpenMenus] = useState([]);

  const toggleMenu = (index) => {
    // Toggle the open/closed state of the clicked menu item
    setOpenMenus((prevOpenMenus) => {
      const updatedOpenMenus = [...prevOpenMenus];
      updatedOpenMenus[index] = !updatedOpenMenus[index];
      return updatedOpenMenus;
    });
  };

  return (
    <div>
      <div
        class={`${
          isMenuVisible ? "" : "hidden"
        } fixed left-0 top-0 w-64 h-full  p-4 z-50 sidebar-menu transition-transform md:block overflow-y-auto bg-gradient-to-b from-cyan-200 via-blue-500 to-purple-500`}
        
      >
        <a
          href="#"
          class="flex items-center justify-center pb-4 border-b border-b-gray-800"
        >
          <h2 class="font-bold text-2xl">
            VENDOR
            <span class="bg-[#f84525] text-white px-2 rounded-md">PORTAL</span>
            <button
              className="float-end pt-1 ml-2 md:hidden"
              onClick={handleMenuVisible}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
                className="feather feather-x"
              >
                <line x1="18" y1="6" x2="6" y2="18"></line>
                <line x1="6" y1="6" x2="18" y2="18"></line>
              </svg>
            </button>
          </h2>
        </a>
        <ul className="mt-4">
          <span className="text-gray-400 font-bold">Menus</span>
          {menuItems.map((menuItem, index) => (
            <li key={index} className="my-2 group">
              <Link
                to={menuItem.link}
                className="flex font-semibold items-center py-2 px-4 text-gray-900 hover:bg-gray-950 hover:text-gray-100 rounded-md group-[.active]:bg-gray-800 group-[.active]:text-white group-[.selected]:bg-gray-950 group-[.selected]:text-gray-100 sidebar-dropdown-toggle"
                onClick={() => toggleMenu(index)}
              >
                <i className={menuItem.icon + " mr-3 text-lg"}></i>
                <span className="text-sm">{menuItem.text}</span>
                {menuItem.subItems && (
                  <i
                    className={`ri-arrow-right-s-line ml-auto ${
                      openMenus[index] ? "rotate-90" : ""
                    }`}
                  ></i>
                )}
              </Link>
              {menuItem.subItems && openMenus[index] && (
                <ul className="pl-7 mt-2">
                  {menuItem.subItems.map((subItem, subIndex) => (
                    <li key={subIndex} className="mb-4">
                      <Link
                        to={subItem.link}
                        className="text-gray-900 text-sm flex items-center hover:text-[#f84525] before:contents-[''] before:w-1 before:h-1 before:rounded-full before:bg-gray-300 before:mr-3"
                      >
                        {subItem.text}
                      </Link>
                    </li>
                  ))}
                </ul>
              )}
            </li>
          ))}
        </ul>
      </div>
      <div
        class={`${
          isMenuVisible ? "" : "hidden"
        }fixed top-0 left-0 w-full h-full bg-black/50 z-40 sidebar-overlay md:hidden`}
      ></div>
    </div>
  );
}