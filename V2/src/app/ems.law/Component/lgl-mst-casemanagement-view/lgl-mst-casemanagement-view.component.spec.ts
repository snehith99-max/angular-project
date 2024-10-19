import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglMstCasemanagementViewComponent } from './lgl-mst-casemanagement-view.component';

describe('LglMstCasemanagementViewComponent', () => {
  let component: LglMstCasemanagementViewComponent;
  let fixture: ComponentFixture<LglMstCasemanagementViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglMstCasemanagementViewComponent]
    });
    fixture = TestBed.createComponent(LglMstCasemanagementViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
