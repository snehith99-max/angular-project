import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstYearendactivitiesComponent } from './sys-mst-yearendactivities.component';

describe('SysMstYearendactivitiesComponent', () => {
  let component: SysMstYearendactivitiesComponent;
  let fixture: ComponentFixture<SysMstYearendactivitiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstYearendactivitiesComponent]
    });
    fixture = TestBed.createComponent(SysMstYearendactivitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
