import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstApprovalassignhierarchyComponent } from './sys-mst-approvalassignhierarchy.component';

describe('SysMstApprovalassignhierarchyComponent', () => {
  let component: SysMstApprovalassignhierarchyComponent;
  let fixture: ComponentFixture<SysMstApprovalassignhierarchyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstApprovalassignhierarchyComponent]
    });
    fixture = TestBed.createComponent(SysMstApprovalassignhierarchyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
