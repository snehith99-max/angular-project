import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptProductconsumptioneportsComponent } from './smr-rpt-productconsumptioneports.component';

describe('SmrRptProductconsumptioneportsComponent', () => {
  let component: SmrRptProductconsumptioneportsComponent;
  let fixture: ComponentFixture<SmrRptProductconsumptioneportsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptProductconsumptioneportsComponent]
    });
    fixture = TestBed.createComponent(SmrRptProductconsumptioneportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
