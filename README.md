# Blackbird.io Remote

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

**Remote** is a Global HR Platform that helps companies hire, manage, and pay their entire team — and more effectively compete in the modern global economy through our comprehensive set of core solutions including, HRIS, payroll, international employment, contractor management, and more.

## Before setting up

Before you can connect you need to make sure that:

- You have a Remote instance
- You have a Remote API token. You can find a detailed guide on how to get it [here](https://remote.com/resources/api/auth-and-authorization).

## Connecting

1. Navigate to Apps, and identify the **Remote** app. You can use search to find it.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Remote connection'.
4. Fill in the `Base URL` field. If you are using production environment, you should use `https://gateway.remote.com`. If you are using sandbox environment, you should use `https://gateway.remote-sandbox.com`.
5. Fill in the `API token` you got from Remote.
6. Click **Connect**.
7. Make sure that connection was added successfully.

![connection](./image/README/connecting.png)

## Actions

### Employees

- **Search employments** Search employments based on specified criteria.
- **Get employment** Get employment by ID.
- **Create employment** Create employment with specified data.
- **Update employment** Update employment by ID with specified data.
- **Invite employment** Invite employment by ID to start the self-enrollment

### Invoices

- **Search invoices** Search invoices based on specified criteria.
- **Get invoice** Get invoice by ID.

### Invoice schedules

- **Search invoice schedules** Search invoice schedules based on specified criteria.
- **Get invoice schedule** Get invoice schedule by ID.
- **Create invoice schedule** Create invoice schedule with specified data.
- **Update invoice schedule** Update invoice schedule by ID with specified data.

## Events

### Employments

- **On employment activated** This event is triggered whenever an employment user is updated to the active status.
- **On employment onboarding completed** This event is triggered whenever an employment user has completed onboarding.
- **On employment details updated** This event is triggered whenever an employment user's details are updated.
- **On employment personal information updated** This event is triggered whenever an employment user's personal information is updated.

### Invoices

- **On invoices approved** This event returns invoices that have been approved since the last polling time. The Remote API allows you to search for invoices that were approved after a specified date. However, this date is in a date-only format, so you should set the polling interval to be at least 1 day.

Note: Invoice events are based on polling. This means that the event will be triggered based on the polling interval you set in the event configuration.

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
