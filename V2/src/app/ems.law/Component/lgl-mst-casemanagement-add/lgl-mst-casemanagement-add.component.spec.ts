import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglMstCasemanagementAddComponent } from './lgl-mst-casemanagement-add.component';

describe('LglMstCasemanagementAddComponent', () => {
  let component: LglMstCasemanagementAddComponent;
  let fixture: ComponentFixture<LglMstCasemanagementAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglMstCasemanagementAddComponent]
    });
    fixture = TestBed.createComponent(LglMstCasemanagementAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
