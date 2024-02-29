import React, { useState, useEffect } from "react";
import axios from "axios";

export default function AddVendorCategory() {
  const [vendorCategoryData, setVendorCategoryData] = useState({
    name: "",
    description: "",
    documentList: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setVendorCategoryData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        `${process.env.REACT_APP_API_URL}/VendorCategory/Add`,
        vendorCategoryData
      );
      if (response.status === 200) alert("Vendor Category Added");
      setVendorCategoryData({
        name: "",
        description: "",
        documentList: "",
      });
    } catch (error) {
      console.error("Error adding VendorCategory:", error.message);
    }
  };
  return (
    <>
      <div class="align-middle inline-block min-w-full shadow overflow-hidden bg-white shadow-dashboard px-8 py-3 pb-8 rounded-bl-lg rounded-br-lg">
        <form onSubmit={handleSubmit} className="max-w-sm mx-auto mt-8">
          <div class="mb-6">
            <label
              for="name"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Name
            </label>
            <input
              type="text"
              id="name"
              name="name"
              value={vendorCategoryData.name}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              for="description"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Description
            </label>
            <input
              type="text"
              id="description"
              name="description"
              value={vendorCategoryData.description}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <div class="mb-6">
            <label
              for="documentList"
              class="block mb-2 text-sm font-medium text-gray-900"
            >
              Document List (comma seperated)
            </label>
            <input
              type="text"
              id="documentList"
              name="documentList"
              value={vendorCategoryData.documentList}
              onChange={handleChange}
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>

          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Add Vendor Category
          </button>
        </form>
      </div>
    </>
  );
}
