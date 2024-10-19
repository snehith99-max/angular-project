import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUsergroupprivilegeaddComponent } from './sys-mst-usergroupprivilegeadd.component';

describe('SysMstUsergroupprivilegeaddComponent', () => {
  let component: SysMstUsergroupprivilegeaddComponent;
  let fixture: ComponentFixture<SysMstUsergroupprivilegeaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUsergroupprivilegeaddComponent]
    });
    fixture = TestBed.createComponent(SysMstUsergroupprivilegeaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
