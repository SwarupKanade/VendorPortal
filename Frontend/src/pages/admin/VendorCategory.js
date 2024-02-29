import React, { useState, useEffect } from "react";
import axios from "axios";
export default function VendorCategory() {
  const [vendorCategory, setVendorCategory] = useState([]);
  useEffect(() => {
    // Fetch vendor data when the component mounts
    const fetchVendorCategoryData = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/VendorCategory/All`
        );

        setVendorCategory(response.data);
      } catch (error) {
        console.error("Error fetching vendor data:", error);
      }
    };

    fetchVendorCategoryData();
  }, []);
  return (
    <>
      <div className="-my-2 py-2 overflow-x-auto sm:-mx-6 sm:px-6 lg:-mx-8 pr-10 lg:px-8 mb-8">
        <div className="align-middle inline-block min-w-full shadow overflow-hidden bg-white shadow-dashboard px-8 pt-3 rounded-bl-lg rounded-br-lg">
          <div className="flex text-2xl font-bold text-gray-500 mb-4 justify-center items-center">
            <h2>Vendor Category</h2>
          </div>
          <table className="min-w-full">
            <thead>
              <tr>
                <th className="px-6 py-3 border-b-2 border-gray-300 text-left leading-4 text-blue-500 tracking-wider">
                  Name
                </th>
                <th className="px-6 py-3 border-b-2 border-gray-300 text-left text-sm leading-4 text-blue-500 tracking-wider">
                  Description
                </th>
                <th className="px-6 py-3 border-b-2 border-gray-300 text-left text-sm leading-4 text-blue-500 tracking-wider">
                  Document List
                </th>
              </tr>
            </thead>
            <tbody className="bg-white">
              {vendorCategory.map((cat) => (
                <tr key={cat.id}>
                  <td className="px-6 py-4 whitespace-no-wrap border-b border-gray-500">
                    <div className="text-sm leading-5 text-blue-900">
                      {cat.name}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-no-wrap border-b border-gray-500">
                    <div className="text-sm leading-5 text-blue-900">
                      {cat.description}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-no-wrap border-b border-gray-500">
                    <div className="text-sm leading-5 text-blue-900">
                      {cat.documentList.split(",").map((doc) => (
                        <span class="bg-blue-100 text-blue-800 text-xs font-medium me-2 px-2.5 py-0.5 rounded">
                          {doc}
                        </span>
                      ))}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}
