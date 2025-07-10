import React, { useState } from 'react'
import type { TaskState } from '../../dto/types'

interface FilterTasksModalProps {   
    setTitleFilter: (value: string) => void
    setDescriptionFilter: (value: string) => void
    setStatusFilter: (value: TaskState | null) => void 
    setCreatedFrom: (value: string) => void
    setCreatedTo: (value: string) => void
    setDueFrom: (value: string) => void
    setDueTo: (value: string) => void
    titleFilter: string
    descriptionFilter: string
    statusFilter: TaskState | null
    createdFrom: string
    createdTo: string
    dueFrom: string
    dueTo: string
}

export default function FilterTasksModal( {titleFilter,
  setTitleFilter,
  descriptionFilter,
  setDescriptionFilter,
  statusFilter,
  setStatusFilter,
  dueFrom,
  setDueFrom,
  dueTo,
  setDueTo,
  createdFrom,
  setCreatedFrom,
  createdTo,
  setCreatedTo
}: FilterTasksModalProps ) {

    return(
         <><div className="grid grid-cols-4 gap-4 mb-6">

                <div className="flex flex-col">
                    <label className="text-sm font-medium mb-1">Filter by title</label>
                    <input
                        type="text"
                        value={titleFilter}
                        onChange={(e) => setTitleFilter(e.target.value)}
                        className="p-2 rounded border" />
                </div>

                <div className="flex flex-col">
                    <label className="text-sm font-medium mb-1">Filter by description</label>
                    <input
                        type="text"
                        value={descriptionFilter}
                        onChange={(e) => setDescriptionFilter(e.target.value)}
                        className="p-2 rounded border" />
                </div>

                <div className="flex flex-col">
                    <label className="text-sm font-medium mb-1">Status</label>
                    <select
                        value={statusFilter ?? ""}
                        onChange={(e) => setStatusFilter(e.target.value === "" ? null : e.target.value as TaskState)}
                        className="p-2 rounded border"
                    >
                        <option value="">All statuses</option>
                        <option value="Pending">Not Started</option>
                        <option value="In progress">In Progress</option>
                        <option value="Completed">Completed</option>
                    </select>
                </div>

                <div className="flex flex-col">
                    <label className="text-sm font-medium mb-1">Created from</label>
                    <input
                        type="date"
                        value={createdFrom}
                        onChange={(e) => setCreatedFrom(e.target.value)}
                        className="p-2 rounded border" />
                </div>

                <div className="flex flex-col">
                    <label className="text-sm font-medium mb-1">Created to</label>
                    <input
                        type="date"
                        value={createdTo}
                        onChange={(e) => setCreatedTo(e.target.value)}
                        className="p-2 rounded border" />
                </div>

                <div className="flex flex-col">
                    <label className="text-sm font-medium mb-1">Due date from</label>
                    <input
                        type="date"
                        value={dueFrom}
                        onChange={(e) => setDueFrom(e.target.value)}
                        className="p-2 rounded border" />
                </div>

                <div className="flex flex-col">
                    <label className="text-sm font-medium mb-1">Due date to</label>
                    <input
                        type="date"
                        value={dueTo}
                        onChange={(e) => setDueTo(e.target.value)}
                        className="p-2 rounded border" />
                </div>
            </div></>
    )
}