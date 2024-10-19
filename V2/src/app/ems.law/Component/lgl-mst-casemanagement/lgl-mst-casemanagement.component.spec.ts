import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglMstCasemanagementComponent } from './lgl-mst-casemanagement.component';

describe('LglMstCasemanagementComponent', () => {
  let component: LglMstCasemanagementComponent;
  let fixture: ComponentFixture<LglMstCasemanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglMstCasemanagementComponent]
    });
    fixture = TestBed.createComponent(LglMstCasemanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
