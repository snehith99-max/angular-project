import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstOrganisationHierarchyComponent } from './sys-mst-organisation-hierarchy.component';

describe('SysMstOrganisationHierarchyComponent', () => {
  let component: SysMstOrganisationHierarchyComponent;
  let fixture: ComponentFixture<SysMstOrganisationHierarchyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstOrganisationHierarchyComponent]
    });
    fixture = TestBed.createComponent(SysMstOrganisationHierarchyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
