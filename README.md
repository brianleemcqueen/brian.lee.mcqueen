brian.lee.mcqueen
=================

Hello,

This repository is a profile of some of the work I have created in the past.  See below for an explanation of what is included:

<em>BEAR</em><br />
BEAR = Bingham Entry And Reporting.  This .NET project was created while working at Bingham McCutchen, LLP, an international law firm based in Boston. The goal of BEAR was to create a project that could be used to spin up many sub-applications quickly.  Each application spun up by BEAR would take on a similar design - Grids with easy data entry.  

In this version of the application, there are the following sub-applications:
* 4qcashforecast
* billTracker
* cas
* cashflow
* exceptionRates
* lockbox
* splitManager

Main Technologies: asp.net, c#, JQuery

Authorization to the applications was handled in a different application combined with location directives on the IIS server.


<em>binaries</em><br />
This folder is setup to store binary files.  Below are explanations of the files included:

* VarianceView.sql 
This is a large SQL statement used to build a report comparing budgeted revenue to actual and forecasted revenue.  A key requirement for the SQL was to include rows that exist in one results and not the other.  This will explain the use of outer joins vs. inner joins

* VIP_Application_Screenshots.pdf
This is a collection of screen shots from applications designed from 2010 to 2014.

* Web Architecture.pdf
This shows the architecture setup while I worked with Vermont Information Processing (VIP).  VIP is an established (over 40 year old) company.  However, when I started with the company in 2010, they did not have a web presence.  My goal was to create this web presence while obtaining data from the legacy system on an IBM iSeries.  Web applications started as single stand alone applications and later were re-architected to a single application with a user manager side application to control authorization.

Brian McQueen

